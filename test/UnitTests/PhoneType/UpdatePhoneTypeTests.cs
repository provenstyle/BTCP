namespace UnitTests.PhoneType
{
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.PhoneType;
    using BibleTraining.Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;

    [TestClass]
    public class UpdatePhoneTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdatePhoneType()
        {
            var phoneType= new PhoneType()
            {
                Id         = 1,
                Name       = "a",
                RowVersion = new byte[] { 0x01 }
            };

            var phoneTypeData = Builder<PhoneTypeData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<PhoneType>())
                .Return(new[] { phoneType }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => phoneType.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new UpdatePhoneType(phoneTypeData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(phoneTypeData.Name, phoneType.Name);

            _context.VerifyAllExpectations();
        }
    }
}
