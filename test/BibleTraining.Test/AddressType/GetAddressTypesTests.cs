namespace BibleTraining.Test.AddressType
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.AddressType;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class GetAddressTypesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetAddressTypes()
        {
            SetupChoices();

            var result = await _mediator.SendAsync(new GetAddressTypes());
            Assert.AreEqual(3, result.AddressTypes.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<AddressType>())
                .Return(TestChoice<AddressType>(3).TestAsync());

            var result = await _mediator.SendAsync(new GetAddressTypes { KeyProperties = true });

            Assert.IsTrue(result.AddressTypes.All(x => x.Name != null));
            Assert.IsTrue(result.AddressTypes.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}