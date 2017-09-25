namespace BibleTraining.Test.EmailType
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;
    using Rhino.Mocks;
    using Entities;
    using Infrastructure;
    using Api.EmailType;
    using Miruken.Mediate;
    
    [TestClass]
    public class GetEmailTypesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetEmailTypes()
        {
            SetupChoices();

            var result = await _handler.Send(new GetEmailTypes());
            Assert.AreEqual(3, result.EmailTypes.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<EmailType>())
                .Return(TestChoice<EmailType>(3).TestAsync());

            var result = await _handler.Send(new GetEmailTypes { KeyProperties = true });

            Assert.IsTrue(result.EmailTypes.All(x => x.Name != null));
            Assert.IsTrue(result.EmailTypes.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}
