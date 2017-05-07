namespace BibleTraining.Test.ContactType
{
    using System.Threading.Tasks;
    using Api.ContactType;
    using Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Test;

    [TestClass]
    public class CreateContactTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreateContactType()
        {
            var contactType = Builder<ContactTypeData>.CreateNew()
                .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                .Build();

            _context.Expect(pg => pg.Add(Arg<EmailType>.Is.Anything))
                .WhenCalled(inv =>
                                {
                                    var entity = (EmailType)inv.Arguments[0];
                                    entity.Id = 1;
                                    entity.RowVersion = new byte[] { 0x01 };
                                    Assert.AreEqual(contactType.Name, entity.Name);
                                    inv.ReturnValue = entity;
                                }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new CreateContactType(contactType));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}