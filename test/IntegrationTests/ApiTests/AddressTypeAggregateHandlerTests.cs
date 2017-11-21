namespace IntegrationTests.ApiTests
{
    using System;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.AddressType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class AddressTypeAggregateHandlerTests : BibleTrainingScenario
    {
        private async Task<AddressTypeData> GetAddressType(int id)
        {
             return (await Handler.Send(new GetAddressTypes(id)))
                .AddressTypes.FirstOrDefault();
        }

        public async Task WithCreated(Func<AddressTypeData, Task> testAction)
        {
            await RollBack(async () =>
             {
                 var addressTypeData = Fixture.Create<AddressTypeData>();
                 var createResult    = await Handler.Send(new CreateAddressType(addressTypeData));
                 var created         = await GetAddressType(createResult.Id ?? -1);
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
                 await Handler.Send(new UpdateAddressType(created));
                 var updated  = await GetAddressType(created.Id ?? -1);

                 Assert.AreEqual(name, updated.Name);
             });
        }

        [TestMethod, ExpectedException(typeof(OptimisticConcurrencyException))]
        public async Task ThrowsOnConcurrentUpdate()
        {
            await WithCreated(async created =>
             {
                 created.Name = "a";
                 await Handler.Send(new UpdateAddressType(created));

                 created.Name = "b";
                 await Handler.Send(new UpdateAddressType(created));
             });
        }

        [TestMethod]
        public async Task CanRemove()
        {
            await WithCreated(async created =>
             {
                 await Handler.Send(new RemoveAddressType(created));
                 var removed = await GetAddressType(created.Id ?? -1);

                 Assert.IsNull(removed);
             });
        }

        [TestMethod, ExpectedException(typeof(OptimisticConcurrencyException))]
        public async Task ThrowsOnConcurrentRemove()
        {
            await WithCreated(async created =>
             {
                 created.Name = "a";
                 await Handler.Send(new UpdateAddressType(created));
                 await Handler.Send(new RemoveAddressType(created));
             });
        }
    }
}
