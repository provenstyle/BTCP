namespace BibleTraining.Test.Address
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Address;
    using Entities;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class GetAddressesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetAddresses()
        {
            SetupChoices();

            var result = await _mediator.SendAsync(new GetAddresses());
            Assert.AreEqual(3, result.Addresses.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<Address>())
                .Return(TestChoice<Address>(3).TestAsync());

            var result = await _mediator.SendAsync(new GetAddresses { KeyProperties = true });

            Assert.IsTrue(result.Addresses.All(x => x.Name != null));
            Assert.IsTrue(result.Addresses.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}