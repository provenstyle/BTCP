namespace BibleTraining.Test.PhoneType 
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Api.PhoneType;
    
    [TestClass]
    public class CreatePhoneTypeIntegrityTests
    {
        private CreatePhoneType createPhoneType;
        private CreatePhoneTypeIntegrity validator;

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

            validator = new CreatePhoneTypeIntegrity();
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
