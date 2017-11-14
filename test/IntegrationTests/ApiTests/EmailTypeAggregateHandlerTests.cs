namespace IntegrationTests.ApiTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.EmailType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class EmailTypeAggregateHandlerTests : BibleTrainingScenario
    {
        private async Task<EmailTypeData> GetEmailType(int id)
        {
             return (await Handler.Send(new GetEmailTypes(id))).EmailTypes.FirstOrDefault();
        }

        public async Task WithCreated(Func<EmailTypeData, Task> testAction)
        {
            await RollBack(async () =>
             {
                 var emailTypeData = Fixture.Create<EmailTypeData>();
                 var createResult = await Handler.Send(new CreateEmailType(emailTypeData));
                 var created = await GetEmailType(createResult.Id ?? -1);
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
                 const string name = "a";

                 created.Name = name;
                 await Handler.Send(new UpdateEmailType(created));
                 var updated = await GetEmailType(created.Id ?? -1);

                 Assert.AreEqual(name, updated.Name);
             });
        }

        [TestMethod]
        public async Task CanRemove()
        {
            await WithCreated(async created =>
             {
                 await Handler.Send(new RemoveEmailType(created));
                 var removed = await GetEmailType(created.Id ?? -1);

                 Assert.IsNull(removed);
             });
        }
    }
}
