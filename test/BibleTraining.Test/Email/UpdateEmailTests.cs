namespace BibleTraining.Test.Email
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Email;
    using Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Test;

    [TestClass]
    public class UpdateEmailTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdateEmail()
        {
            var email= new Email()
            {
                Id         = 1,
                Address    = "A",
                RowVersion = new byte[] { 0x01 }
            };

            var emailData = Builder<EmailData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Email>())
                .Return(new[] { email }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => email.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new UpdateEmail(emailData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(emailData.Address, email.Address);

            _context.VerifyAllExpectations();
        }
    }
}