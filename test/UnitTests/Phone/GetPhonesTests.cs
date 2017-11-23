namespace UnitTests.Phone
{
    using System.Threading.Tasks;
    using BibleTraining.Api.Phone;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;

    [TestClass]
    public class GetPhonesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetPhones()
        {
            SetupChoices();

            var result = await _handler.Send(new GetPhones());
            Assert.AreEqual(3, result.Phones.Length);

            _context.VerifyAllExpectations();
        }
    }
}
