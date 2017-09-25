namespace BibleTraining.Test.AddressType
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using FizzWare.NBuilder;
    using Rhino.Mocks;
    using Entities;
    using Infrastructure;
    using Api.AddressType;
    using Miruken.Mediate;

    [TestClass]
    public class RemoveAddressTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldRemoveAddressType()
        {
            var entity = new AddressType
            {
                Id         = 1,
                Name       = "a",
                RowVersion = new byte[] { 0x01 }
            };

            var addressTypeData = Builder<AddressTypeData>.CreateNew()
                .With(pg => pg.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(pg => pg.AsQueryable<AddressType>())
                .Return(new[] { entity }.AsQueryable().TestAsync());

            _context.Expect(c => c.Remove(entity))
                .Return(entity);

            _context.Expect(c => c.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new RemoveAddressType(addressTypeData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}
