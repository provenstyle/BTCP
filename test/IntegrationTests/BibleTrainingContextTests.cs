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
    public class BibleTrainingContextTests : BibleTrainingScenario
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
                 emailTypeData.Created  = null;
                 emailTypeData.Modified = null;
                 var createResult = await Handler.Send(new CreateEmailType(emailTypeData));
                 var created = await GetEmailType(createResult.Id ?? -1);
                 await testAction(created);
             });
        }

        [TestMethod]
        public async Task PopulatesCreatedAndModifiedDatesOnSaveWhenEntityIsCreated()
        {
            await WithCreated(created =>
             {
                 Assert.IsNotNull(created.Created);
                 Assert.IsNotNull(created.Modified);
                 return Task.FromResult(true);
             });
        }

        [TestMethod]
        public async Task UpdatesModifiedDateOnSaveWhenEntityIsUpdated()
        {
            await WithCreated(async created =>
             {
                 created.Name = "updated";
                 await Handler.Send(new UpdateEmailType(created));
                 var updated = await GetEmailType(created.Id ?? -1);
                 Assert.IsTrue(updated.Modified > created.Modified);
             });
        }
    }
}
