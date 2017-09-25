namespace BibleTraining.Test.EmailType
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;
    using FizzWare.NBuilder;
    using Rhino.Mocks;
    using Api.EmailType;
    using Entities;
    using Infrastructure;
    using Miruken.Mediate;
    
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
