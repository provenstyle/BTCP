namespace UnitTests.EmailType
{
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.EmailType;
    using BibleTraining.Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;

    [TestClass]
    public class UpdateEmailTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdateEmailType()
        {
            var emailType= new EmailType()
            {
                Id         = 1,
                Name       = "a",
                RowVersion = new byte[] { 0x01 }
            };

            var emailTypeData = Builder<EmailTypeData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<EmailType>())
                .Return(new[] { emailType }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => emailType.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new UpdateEmailType(emailTypeData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(emailTypeData.Name, emailType.Name);

            _context.VerifyAllExpectations();
        }
    }
}
