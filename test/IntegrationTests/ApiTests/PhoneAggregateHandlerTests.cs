namespace IntegrationTests.ApiTests
{
    using System;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.Person;
    using BibleTraining.Api.Phone;
    using BibleTraining.Api.PhoneType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class PhoneAggregateHandlerTests : BibleTrainingScenario
    {
        private const string UpdatedNumber = "1 940 395 1111";
        private const string SecondUpdatedNumber = "1 940 395 2222";

        private async Task<PhoneData> GetPhone(int id)
        {
             return (await Handler.Send(new GetPhones(id))).Phones.FirstOrDefault();
        }

        public async Task WithCreated(Func<PhoneData, Task> testAction)
        {
            await RollBack(async () =>
             {
                 int phoneTypeId;
                 int personId;
                 using (var scope = Repository.Scopes.Create())
                 {
                     var phoneTypeResult = await Handler.Send(new CreatePhoneType(Fixture.Create<PhoneTypeData>()));
                     var personResult    = await Handler.Send(new CreatePerson(Fixture.Create<PersonData>()));
                     scope.SaveChanges();
                     phoneTypeId = phoneTypeResult.Id ?? -1;
                     personId    = personResult.Id ?? -1;
                 }

                 var data = Fixture.Create<PhoneData>();
                 data.PhoneTypeId = phoneTypeId;
                 data.PersonId = personId;

                 var createResult = await Handler.Send(new CreatePhone(data));
                 var created = await GetPhone(createResult.Id ?? -1);
                 await testAction(created);
             });
        }

        [TestMethod]
        public async Task CanAdd()
        {
            await WithCreated(created =>
              {
                  Assert.IsNotNull(created);
                  return Task.FromResult(true);
              });
        }

        [TestMethod]
        public async Task CanUpdate()
        {
            await WithCreated(async created =>
             {
                 created.Number = UpdatedNumber;
                 await Handler.Send(new UpdatePhone(created));
                 var updated = await GetPhone(created.Id ?? -1);

                 Assert.AreEqual(UpdatedNumber, updated.Number);
             });
        }

        [TestMethod]
        public async Task CanRemove()
        {
            await WithCreated(async created =>
             {
                 await Handler.Send(new RemovePhone(created));
                 var removed = await GetPhone(created.Id ?? -1);

                 Assert.IsNull(removed);
             });
        }

        [TestMethod, ExpectedException(typeof(OptimisticConcurrencyException))]
        public async Task ThrowsOnConcurrentUpdate()
        {
            await WithCreated(async created =>
             {
                 created.Number = UpdatedNumber;
                 await Handler.Send(new UpdatePhone(created));

                 created.Number = SecondUpdatedNumber;
                 await Handler.Send(new UpdatePhone(created));
             });
        }

        [TestMethod, ExpectedException(typeof(OptimisticConcurrencyException))]
        public async Task ThrowsOnConcurrentRemove()
        {
            await WithCreated(async created =>
             {
                 created.Number = UpdatedNumber;
                 await Handler.Send(new UpdatePhone(created));
                 await Handler.Send(new RemovePhone(created));
             });
        }
    }
}
