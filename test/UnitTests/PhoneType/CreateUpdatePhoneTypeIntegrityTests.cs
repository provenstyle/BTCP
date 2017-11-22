namespace UnitTests.PhoneType 
{
    using BibleTraining.Api.PhoneType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateUpdatePhoneTypeIntegrityTests
    {
        private CreatePhoneType createPhoneType;
        private CreateUpdatePhoneTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createPhoneType =  new CreatePhoneType
            {
                 Resource = new PhoneTypeData
                 {
                    Name = "a"
                 }
            };

            validator = new CreateUpdatePhoneTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createPhoneType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            createPhoneType.Resource.Name = string.Empty;
            var result = validator.Validate(createPhoneType);
            Assert.IsFalse(result.IsValid);
        }
    }
}
