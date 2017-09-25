namespace $ApplicationName$.Test.$Entity$
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;
    using Rhino.Mocks;
    using Entities;
    using Infrastructure;
    using Api.$Entity$;
    using Miruken.Mediate;
    
    [TestClass]
    public class Get$EntityPlural$Tests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGet$EntityPlural$()
        {
            SetupChoices();

            var result = await _handler.Send(new Get$EntityPlural$());
            Assert.AreEqual(3, result.$EntityPlural$.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<$Entity$>())
                .Return(TestChoice<$Entity$>(3).TestAsync());

            var result = await _handler.Send(new Get$EntityPlural$ { KeyProperties = true });

            Assert.IsTrue(result.$EntityPlural$.All(x => x.Name != null));
            Assert.IsTrue(result.$EntityPlural$.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}
