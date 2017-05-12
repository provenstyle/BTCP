namespace BibleTraining.Test.Phone
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Phone;
    using Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class UpdatePhoneTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdatePhone()
        {
            var phone= new Phone()
            {
                Id         = 1,
                Name       = "a",
                RowVersion = new byte[] { 0x01 }
            };

            var phoneData = Builder<PhoneData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Phone>())
                .Return(new[] { phone }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => phone.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new UpdatePhone(phoneData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(phoneData.Name, phone.Name);

            _context.VerifyAllExpectations();
        }
    }
}