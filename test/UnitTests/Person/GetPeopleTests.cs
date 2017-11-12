namespace UnitTests.Person
{
    using System.Threading.Tasks;
    using BibleTraining.Api.Person;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;
    using UnitTests;

    [TestClass]
    public class GetPeopleTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetPeople()
        {
            SetupChoices();

            var result = await _handler.Send(new GetPeople());
            Assert.AreEqual(3, result.People.Length);

            _context.VerifyAllExpectations();
        }
    }
}