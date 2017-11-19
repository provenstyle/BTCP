namespace UnitTests.Address
{
    using System.Threading.Tasks;
    using BibleTraining.Api.Address;
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;
    using Ploeh.AutoFixture;

    [TestClass]
    public class CreateAddressTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreateAddress()
        {
            var address = Fixture.Create<AddressData>();
            address.AddressTypeId = 1;
            address.PersonId = 1;

            _context.Expect(pg => pg.Add(Arg<Address>.Is.Anything))
                .WhenCalled(inv =>
                    {
                        var entity = (Address)inv.Arguments[0];
                        entity.Id         = 1;
                        entity.RowVersion = new byte[] { 0x01 };
                        Assert.AreEqual(address.Name, entity.Name);
                        inv.ReturnValue = entity;
                    }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new CreateAddress(address));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}