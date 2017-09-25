namespace BibleTraining.Test.Email
{
    using System.Threading.Tasks;
    using Api.Email;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;
    using Test;

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