namespace $ApplicationName$.Test.$Entity$
{
    using System.Threading.Tasks;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Entity;
    using Infrastructure;

    [TestClass]
    public class Create$Entity$Tests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreate$Entity$()
        {
            var $entityLowercase$ = Builder<$Entity$Data>.CreateNew()
                  .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                  .Build();

            _context.Expect(pg => pg.Add(Arg<$Entity$>.Is.Anything))
                  .WhenCalled(inv =>
                  {
                      var entity = ($Entity$)inv.Arguments[0];
                      entity.Id         = 1;
                      entity.RowVersion = new byte[] { 0x01 };
                      Assert.AreEqual($entityLowercase$.Name, entity.Name);
                      inv.ReturnValue = entity;
                  }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new Create$Entity$($entityLowercase$));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}
