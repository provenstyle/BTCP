namespace UnitTests.Email
{
    using System.Threading.Tasks;
    using BibleTraining.Api.Email;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;

    [TestClass]
    public class GetEmailsTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetEmails()
        {
            SetupChoices();

            var result = await _handler.Send(new GetEmails());
            Assert.AreEqual(3, result.Emails.Length);

            _context.VerifyAllExpectations();
        }
    }
}