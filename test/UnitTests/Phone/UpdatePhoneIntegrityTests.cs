namespace UnitTests.Phone
{
    using BibleTraining.Api.Phone;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdatePhoneIntegrityTests
    {
        private UpdatePhone updatePhone;
        private UpdatePhoneIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updatePhone =  new UpdatePhone
            {
                 Resource = new PhoneData
                 {
                    Id   = 1,
                    Name = "a"
                 }
            };

            validator = new UpdatePhoneIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updatePhone);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            updatePhone.Resource.Name = string.Empty;
            var result = validator.Validate(updatePhone);
            Assert.IsFalse(result.IsValid);
        }
    }
}
