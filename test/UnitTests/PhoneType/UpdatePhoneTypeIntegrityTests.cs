namespace UnitTests.PhoneType
{
    using BibleTraining.Api.PhoneType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdatePhoneTypeIntegrityTests
    {
        private UpdatePhoneType updatePhoneType;
        private UpdatePhoneTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updatePhoneType =  new UpdatePhoneType
            {
                 Resource = new PhoneTypeData
                 {
                    Id   = 1,
                    Name = "a"
                 }
            };

            validator = new UpdatePhoneTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updatePhoneType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            updatePhoneType.Resource.Name = string.Empty;
            var result = validator.Validate(updatePhoneType);
            Assert.IsFalse(result.IsValid);
        }
    }
}
