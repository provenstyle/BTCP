namespace BibleTraining.Test.ContactType
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.ContactType;
    using Entities;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Test;

    [TestClass]
    public class GetContactTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetContactTypes()
        {
            SetupChoices();

            var result = await _mediator.SendAsync(new GetContactTypes());
            Assert.AreEqual(3, result.ContactTypes.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<EmailType>())
                .Return(TestChoice<EmailType>(3).TestAsync());

            var result = await _mediator.SendAsync(new GetContactTypes { KeyProperties = true });

            Assert.IsTrue(result.ContactTypes.All(x => x.Name != null));
            Assert.IsTrue(result.ContactTypes.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}