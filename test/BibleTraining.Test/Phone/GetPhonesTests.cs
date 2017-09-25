namespace BibleTraining.Test.Phone
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;
    using Rhino.Mocks;
    using Entities;
    using Infrastructure;
    using Api.Phone;
    using Miruken.Mediate;
    
    [TestClass]
    public class GetPhonesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetPhones()
        {
            SetupChoices();

            var result = await _handler.Send(new GetPhones());
            Assert.AreEqual(3, result.Phones.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<Phone>())
                .Return(TestChoice<Phone>(3).TestAsync());

            var result = await _handler.Send(new GetPhones { KeyProperties = true });

            Assert.IsTrue(result.Phones.All(x => x.Name != null));
            Assert.IsTrue(result.Phones.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}
