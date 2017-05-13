namespace BibleTraining.Test.Phone
{
    using Api.Phone;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreatePhoneIntegrityTests
    {
        private CreatePhone createPhone;
        private CreatePhoneIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createPhone =  new CreatePhone
            {
                Resource = new PhoneData
                {
                    Name        = "a",
                    PhoneTypeId = 1
                }
            };

            validator = new CreatePhoneIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createPhone);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHavePhoneTypeId()
        {
            createPhone.Resource.PhoneTypeId = null;
            var result = validator.Validate(createPhone);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            createPhone.Resource.Name = string.Empty;
            var result = validator.Validate(createPhone);
            Assert.IsFalse(result.IsValid);
        }
    }
}