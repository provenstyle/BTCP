namespace BibleTraining.Test.Phone
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;
    using FizzWare.NBuilder;
    using Rhino.Mocks;
    using Api.Phone;
    using Entities;
    using Infrastructure;
    using Miruken.Mediate;
    
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

            var result = await _handler.Send(new UpdatePhone(phoneData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(phoneData.Name, phone.Name);

            _context.VerifyAllExpectations();
        }
    }
}
