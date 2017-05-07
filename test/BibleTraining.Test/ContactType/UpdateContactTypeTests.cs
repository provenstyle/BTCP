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
    public class UpdateContactTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdateContactType()
        {
            var contactType = new EmailType()
            {
                Id = 1,
                Name = "ABC",
                RowVersion = new byte[] { 0x01 }
            };

            var contactTypeData = Builder<ContactTypeData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<EmailType>())
                .Return(new[] { contactType }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => contactType.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new UpdateContactType(contactTypeData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(contactTypeData.Name, contactType.Name);

            _context.VerifyAllExpectations();
        }
    }
}