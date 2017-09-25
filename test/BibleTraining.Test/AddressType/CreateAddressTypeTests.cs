namespace BibleTraining.Test.AddressType
{
    using System.Threading.Tasks;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Entities;
    using Api.AddressType;
    using Miruken.Mediate;
    
    [TestClass]
    public class CreateAddressTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreateAddressType()
        {
            var addressType = Builder<AddressTypeData>.CreateNew()
                  .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                  .Build();

            _context.Expect(pg => pg.Add(Arg<AddressType>.Is.Anything))
                  .WhenCalled(inv =>
                  {
                      var entity = (AddressType)inv.Arguments[0];
                      entity.Id         = 1;
                      entity.RowVersion = new byte[] { 0x01 };
                      Assert.AreEqual(addressType.Name, entity.Name);
                      inv.ReturnValue = entity;
                  }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new CreateAddressType(addressType));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}
