namespace IntegrationTests.ApiTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.PhoneType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class PhoneTypeAggregateHandlerTests : BibleTrainingScenario
    {
        private async Task<PhoneTypeData> GetPhoneType(int id)
        {
             return (await Handler.Send(new GetPhoneTypes(id))).PhoneTypes.FirstOrDefault();
        }

        public async Task WithCreated(Func<PhoneTypeData, Task> testAction)
        {
            await RollBack(async () =>
             {
                 var phoneTypeData = Fixture.Create<PhoneTypeData>();
                 var createResult = await Handler.Send(new CreatePhoneType(phoneTypeData));
                 var created = await GetPhoneType(createResult.Id ?? -1);
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
                 await Handler.Send(new UpdatePhoneType(created));
                 var updated = await GetPhoneType(created.Id ?? -1);

                 Assert.AreEqual(name, updated.Name);
             });
        }

        [TestMethod]
        public async Task CanRemove()
        {
            await WithCreated(async created =>
             {
                 await Handler.Send(new RemovePhoneType(created));
                 var removed = await GetPhoneType(created.Id ?? -1);

                 Assert.IsNull(removed);
             });
        }
    }
}
