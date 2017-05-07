namespace BibleTraining.Test.Email
{
    using System.Threading.Tasks;
    using Api.Email;
    using Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Test;

    [TestClass]
    public class CreateEmailTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreateEmail()
        {
            var email = Builder<EmailData>.CreateNew()
                .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                .Build();

            _context.Expect(pg => pg.Add(Arg<Email>.Is.Anything))
                .WhenCalled(inv =>
                                {
                                    var entity = (Email)inv.Arguments[0];
                                    entity.Id         = 1;
                                    entity.RowVersion = new byte[] { 0x01 };
                                    Assert.AreEqual(email.Address, entity.Address);
                                    inv.ReturnValue = entity;
                                }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new CreateEmail(email));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}