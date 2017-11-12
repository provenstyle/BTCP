namespace BibleTraining.Test.Person
{
    using System.Threading.Tasks;
    using Api.Person;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Test;

    [TestClass]
    public class GetPeopleTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetPeople()
        {
            SetupChoices();

            var result = await _mediator.SendAsync(new GetPeople());
            Assert.AreEqual(3, result.People.Length);

            _context.VerifyAllExpectations();
        }
    }
}