namespace UnitTests.$Entity$
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using FizzWare.NBuilder;
    using Rhino.Mocks;
    using $ApplicationName$.Entities;
    using Infrastructure;
    using $ApplicationName$.Api.$Entity$;
    using Miruken.Mediate;

    [TestClass]
    public class Remove$Entity$Tests : TestScenario
    {
        [TestMethod]
        public async Task ShouldRemove$Entity$()
        {
            var entity = new $Entity$
            {
                Id         = 1,
                Name       = "a",
                RowVersion = new byte[] { 0x01 }
            };

            var $entityLowercase$Data = Builder<$Entity$Data>.CreateNew()
                .With(pg => pg.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(pg => pg.AsQueryable<$Entity$>())
                .Return(new[] { entity }.AsQueryable().TestAsync());

            _context.Expect(c => c.Remove(entity))
                .Return(entity);

            _context.Expect(c => c.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _handler.Send(new Remove$Entity$($entityLowercase$Data));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}
