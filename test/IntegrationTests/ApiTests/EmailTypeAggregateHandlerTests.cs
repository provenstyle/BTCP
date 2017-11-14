namespace IntegrationTests.ApiTests
{
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
        [TestMethod]
        public async Task CanAdd()
        {
            await RollBack(async () =>
             {
                 var emailTypeData = Fixture.Create<EmailTypeData>();
                 var createResult = await Handler.Send(new CreateEmailType(emailTypeData));
                 var id = createResult.Id.Value;
                 var emailType = await GetEmailType(id);

                 Assert.AreEqual(emailTypeData.Name, emailType.Name);
             });
        }

        [TestMethod]
        public async Task CanUpdate()
        {
            await RollBack(async () =>
             {
                 var emailTypeData = Fixture.Create<EmailTypeData>();
                 var createResult = await Handler.Send(new CreateEmailType(emailTypeData));
                 var id = createResult.Id.Value;
                 var emailType = await GetEmailType(id);

                 emailType.Name = "a";
                 await Handler.Send(new UpdateEmailType(emailType));
                 emailType = await GetEmailType(id);

                 Assert.AreEqual("a", emailType.Name);
             });
        }

        [TestMethod]
        public async Task CanRemove()
        {
            await RollBack(async () =>
             {
                 var emailTypeData = Fixture.Create<EmailTypeData>();
                 var createResult = await Handler.Send(new CreateEmailType(emailTypeData));
                 var id = createResult.Id.Value;
                 var emailType = await GetEmailType(id);

                 await Handler.Send(new RemoveEmailType(emailType));
                 emailType = await GetEmailType(id);

                 Assert.IsNull(emailType);
             });
        }

        private async Task<EmailTypeData> GetEmailType(int id)
        {
             return (await Handler.Send(new GetEmailTypes(id))).EmailTypes.FirstOrDefault();
        }
    }
}
