namespace UnitTests.Address
{
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.Address;
    using BibleTraining.Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;

    [TestClass]
    public class UpdateAddressTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdateAddress()
        {
            var address= new Address()
            {
                Id         = 1,
                Name       = "a",
                RowVersion = new byte[] { 0x01 }
            };

            var addressData = Builder<AddressData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Address>())
                .Return(new[] { address }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => address.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new UpdateAddress(addressData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(addressData.Name, address.Name);

            _context.VerifyAllExpectations();
        }
    }
}