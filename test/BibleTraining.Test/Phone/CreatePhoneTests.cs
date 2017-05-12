namespace BibleTraining.Test.Phone
{
    using System.Threading.Tasks;
    using Api.Phone;
    using Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class CreatePhoneTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreatePhone()
        {
            var phone = Builder<PhoneData>.CreateNew()
                .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                .Build();

            _context.Expect(pg => pg.Add(Arg<Phone>.Is.Anything))
                .WhenCalled(inv =>
                                {
                                    var entity = (Phone)inv.Arguments[0];
                                    entity.Id         = 1;
                                    entity.RowVersion = new byte[] { 0x01 };
                                    Assert.AreEqual(phone.Name, entity.Name);
                                    inv.ReturnValue = entity;
                                }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new CreatePhone(phone));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}