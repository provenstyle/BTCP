namespace BibleTraining.Test.PhoneType
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;
    using Rhino.Mocks;
    using Entities;
    using Infrastructure;
    using Api.PhoneType;
    
    [TestClass]
    public class GetPhoneTypesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetPhoneTypes()
        {
            SetupChoices();

            var result = await _mediator.SendAsync(new GetPhoneTypes());
            Assert.AreEqual(3, result.PhoneTypes.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<PhoneType>())
                .Return(TestChoice<PhoneType>(3).TestAsync());

            var result = await _mediator.SendAsync(new GetPhoneTypes { KeyProperties = true });

            Assert.IsTrue(result.PhoneTypes.All(x => x.Name != null));
            Assert.IsTrue(result.PhoneTypes.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}
