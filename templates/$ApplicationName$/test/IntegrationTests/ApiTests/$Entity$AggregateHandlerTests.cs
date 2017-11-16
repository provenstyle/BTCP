namespace IntegrationTests.ApiTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.$Entity$;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Scenarios;
    using Ploeh.AutoFixture;

    [TestClass]
    public class $Entity$AggregateHandlerTests : BibleTrainingScenario
    {
        private async Task<$Entity$Data> Get$Entity$(int id)
        {
             return (await Handler.Send(new Get$Entity$s(id))).$Entity$s.FirstOrDefault();
        }

        public async Task WithCreated(Func<$Entity$Data, Task> testAction)
        {
            await RollBack(async () =>
             {
                 var data = Fixture.Create<$Entity$Data>();
                 var createResult = await Handler.Send(new Create$Entity$(data));
                 var created = await Get$Entity$(createResult.Id ?? -1);
                 Assert.IsNotNull(created);

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
                 await Handler.Send(new Update$Entity$(created));
                 var updated = await Get$Entity$(created.Id ?? -1);

                 Assert.AreEqual(name, updated.Name);
             });
        }

        [TestMethod]
        public async Task CanRemove()
        {
            await WithCreated(async created =>
             {
                 await Handler.Send(new Remove$Entity$(created));
                 var removed = await Get$Entity$(created.Id ?? -1);

                 Assert.IsNull(removed);
             });
        }
    }
}
