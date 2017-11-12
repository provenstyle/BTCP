namespace UnitTests.PhoneType
{
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining.Api.PhoneType;
    using BibleTraining.Entities;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Rhino.Mocks;

    [TestClass]
    public class GetPhoneTypesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetPhoneTypes()
        {
            SetupChoices();

            var result = await _handler.Send(new GetPhoneTypes());
            Assert.AreEqual(3, result.PhoneTypes.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<PhoneType>())
                .Return(TestChoice<PhoneType>(3).TestAsync());

            var result = await _handler.Send(new GetPhoneTypes { KeyProperties = true });

            Assert.IsTrue(result.PhoneTypes.All(x => x.Name != null));
            Assert.IsTrue(result.PhoneTypes.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}
