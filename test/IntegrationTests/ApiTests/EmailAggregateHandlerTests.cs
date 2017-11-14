namespace IntegrationTests.ApiTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.Email;
    using BibleTraining.Api.EmailType;
    using BibleTraining.Api.Person;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class EmailAggregateHandlerTests : BibleTrainingScenario
    {
        private async Task<EmailData> GetEmail(int id)
        {
             return (await Handler.Send(new GetEmails(id))).Emails.FirstOrDefault();
        }

        public async Task WithCreated(Func<int, Task> testAction)
        {
            await RollBack(async () =>
             {
                 int emailTypeId;
                 int personId;
                 using (var scope = Repository.Scopes.Create())
                 {
                     var emailTypeResult = await Handler.Send(new CreateEmailType(Fixture.Create<EmailTypeData>()));
                     var personResult    = await Handler.Send(new CreatePerson(Fixture.Create<PersonData>()));
                     scope.SaveChanges();
                     emailTypeId = emailTypeResult.Id ?? -1;
                     personId    = personResult.Id ?? -1;
                 }

                 var data = Fixture.Create<EmailData>();
                 data.EmailTypeId = emailTypeId;
                 data.PersonId    = personId;

                 var createResult = await Handler.Send(new CreateEmail(data));
                 await testAction(createResult.Id ?? 0);
             });
        }

        [TestMethod]
        public async Task CanAdd()
        {
            await WithCreated(async id =>
            {
                 var created = await GetEmail(id);
                 Assert.IsNotNull(created);
            });
        }

        [TestMethod]
        public async Task CanUpdate()
        {
            await WithCreated(async id =>
              {
                 var created = await GetEmail(id);
                 var address = "a@a.com";
                 created.Address = address;
                 await Handler.Send(new UpdateEmail(created));
                 var updated = await GetEmail(id);

                 Assert.AreEqual(address, updated.Address);
              });
        }

        [TestMethod]
        public async Task CanRemove()
        {
            await WithCreated(async id =>
              {
                 var created = await GetEmail(id);
                 Assert.IsNotNull(created);
                 await Handler.Send(new RemoveEmail(created));
                 var removed = await GetEmail(id);

                 Assert.IsNull(removed);
              });
        }
    }
}
