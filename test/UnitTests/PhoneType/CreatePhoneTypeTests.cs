namespace UnitTests.PhoneType
{
    using System.Threading.Tasks;
    using BibleTraining.Api.PhoneType;
    using BibleTraining.Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;

    [TestClass]
    public class CreatePhoneTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreatePhoneType()
        {
            var phoneType = Builder<PhoneTypeData>.CreateNew()
                  .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                  .Build();

            _context.Expect(pg => pg.Add(Arg<PhoneType>.Is.Anything))
                  .WhenCalled(inv =>
                  {
                      var entity = (PhoneType)inv.Arguments[0];
                      entity.Id         = 1;
                      entity.RowVersion = new byte[] { 0x01 };
                      Assert.AreEqual(phoneType.Name, entity.Name);
                      inv.ReturnValue = entity;
                  }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new CreatePhoneType(phoneType));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}
