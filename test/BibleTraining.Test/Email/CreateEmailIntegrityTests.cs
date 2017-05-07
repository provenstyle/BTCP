namespace BibleTraining.Test.Email
{
    using Api.Email;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateEmailIntegrityTests
    {
        private CreateEmail createEmail;
        private CreateEmailIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createEmail =  new CreateEmail
            {
                Resource = new EmailData
                {
                    Address = "my text"
                }
            };

            validator = new CreateEmailIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createEmail);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveText()
        {
            createEmail.Resource.Address = string.Empty;
            var result = validator.Validate(createEmail);
            Assert.IsFalse(result.IsValid);
        }
    }
}