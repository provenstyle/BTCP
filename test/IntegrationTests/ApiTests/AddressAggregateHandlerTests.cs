namespace IntegrationTests.ApiTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.Address;
    using BibleTraining.Api.AddressType;
    using BibleTraining.Api.Person;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class AddressAggregateHandlerTests : BibleTrainingScenario
    {
        private async Task<AddressData> GetAddress(int id)
        {
             return (await Handler.Send(new GetAddresses(id))).Addresses.FirstOrDefault();
        }

        public async Task WithCreated(Func<int, Task> testAction)
        {
            await RollBack(async () =>
             {
                 int addressTypeId;
                 int personId;
                 using (var scope = Repository.Scopes.Create())
                 {
                     var addressTypeResult = await Handler.Send(new CreateAddressType(Fixture.Create<AddressTypeData>()));
                     var personResult    = await Handler.Send(new CreatePerson(Fixture.Create<PersonData>()));
                     scope.SaveChanges();
                     addressTypeId = addressTypeResult.Id ?? -1;
                     personId    = personResult.Id ?? -1;
                 }

                 var data = Fixture.Create<AddressData>();
                 data.AddressTypeId = addressTypeId;
                 data.PersonId    = personId;

                 var createResult = await Handler.Send(new CreateAddress(data));
                 await testAction(createResult.Id ?? 0);
             });
        }

        [TestMethod]
        public async Task CanAdd()
        {
            await WithCreated(async id =>
            {
                 var created = await GetAddress(id);
                 Assert.IsNotNull(created);
            });
        }

        [TestMethod]
        public async Task CanUpdate()
        {
            await WithCreated(async id =>
              {
                 var created = await GetAddress(id);
                 var name = "a";
                 created.Name = name;
                 await Handler.Send(new UpdateAddress(created));
                 var updated = await GetAddress(id);

                 Assert.AreEqual(name, updated.Name);
              });
        }

        [TestMethod]
        public async Task CanRemove()
        {
            await WithCreated(async id =>
              {
                 var created = await GetAddress(id);
                 Assert.IsNotNull(created);
                 await Handler.Send(new RemoveAddress(created));
                 var removed = await GetAddress(id);

                 Assert.IsNull(removed);
              });
        }
    }
}
