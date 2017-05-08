namespace BibleTraining.Test.EmailType
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.EmailType;
    using Api.EmailType;
    using Entities;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Test;

    [TestClass]
    public class GetEmailTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetEmailTypes()
        {
            SetupChoices();

            var result = await _mediator.SendAsync(new GetEmailTypes());
            Assert.AreEqual(3, result.EmailTypes.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<EmailType>())
                .Return(TestChoice<EmailType>(3).TestAsync());

            var result = await _mediator.SendAsync(new GetEmailTypes { KeyProperties = true });

            Assert.IsTrue(result.EmailTypes.All(x => x.Name != null));
            Assert.IsTrue(result.EmailTypes.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}