namespace BibleTraining.Test.Email
{
    using Api.Email;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdateEmailIntegrityTests
    {
        private UpdateEmail updateEmail;
        private UpdateEmailIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updateEmail =  new UpdateEmail
            {
                Resource = new EmailData
                {
                    Address = "my text"
                }
            };

            validator = new UpdateEmailIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updateEmail);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveText()
        {
            updateEmail.Resource.Address = string.Empty;
            var result = validator.Validate(updateEmail);
            Assert.IsFalse(result.IsValid);
        }
    }
}