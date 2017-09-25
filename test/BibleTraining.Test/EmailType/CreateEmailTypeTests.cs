namespace BibleTraining.Test.EmailType
{
    using System.Threading.Tasks;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Entities;
    using Api.EmailType;
    using Miruken.Mediate;
    
    [TestClass]
    public class CreateEmailTypeTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreateEmailType()
        {
            var emailType = Builder<EmailTypeData>.CreateNew()
                  .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                  .Build();

            _context.Expect(pg => pg.Add(Arg<EmailType>.Is.Anything))
                  .WhenCalled(inv =>
                  {
                      var entity = (EmailType)inv.Arguments[0];
                      entity.Id         = 1;
                      entity.RowVersion = new byte[] { 0x01 };
                      Assert.AreEqual(emailType.Name, entity.Name);
                      inv.ReturnValue = entity;
                  }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new CreateEmailType(emailType));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}
