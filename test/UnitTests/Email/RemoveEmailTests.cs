namespace UnitTests.Email
{
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.Email;
    using BibleTraining.Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;

    [TestClass]
    public class RemoveEmailTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldRemoveEmail()
        {
            var entity = new Email
            {
                Id         = 1,
                Address       = "ABC",
                RowVersion = new byte[] { 0x01 }
            };

            var emailData = Builder<EmailData>.CreateNew()
                .With(pg => pg.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(pg => pg.AsQueryable<Email>())
                .Return(new[] { entity }.AsQueryable().TestAsync());

            _context.Expect(c => c.Remove(entity))
                .Return(entity);

            _context.Expect(c => c.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new RemoveEmail(emailData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}