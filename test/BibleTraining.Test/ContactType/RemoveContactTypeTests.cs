namespace BibleTraining.Test.ContactType
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.ContactType;
    using Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Test;

    [TestClass]
    public class RemoveContactTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldRemoveContactType()
        {
            var entity = new EmailType
            {
                Id = 1,
                Name = "ABC",
                RowVersion = new byte[] { 0x01 }
            };

            var contactTypeData = Builder<ContactTypeData>.CreateNew()
                .With(pg => pg.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(pg => pg.AsQueryable<EmailType>())
                .Return(new[] { entity }.AsQueryable().TestAsync());

            _context.Expect(c => c.Remove(entity))
                .Return(entity);

            _context.Expect(c => c.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new RemoveContactType(contactTypeData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}