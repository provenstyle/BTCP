namespace UnitTests.Email
{
    using BibleTraining.Api.Email;
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
                    PersonId    = 1,
                    EmailTypeId = 1,
                    Address     = "a@a.com"
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
        public void MustHaveAddress()
        {
            createEmail.Resource.Address = string.Empty;
            var result = validator.Validate(createEmail);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHaveValidAddress()
        {
            var addresses = new []{"a", "a.a", "a@a", "a@a.", "a.com"};
            foreach (var address in addresses)
            {
                createEmail.Resource.Address = address;
                var result = validator.Validate(createEmail);
                Assert.IsFalse(result.IsValid);
            }
        }

        //[TestMethod]
        //public void MustHavePersonId()
        //{
        //    HandleMethod.Composer = new Stash();

        //    createEmail.Resource.PersonId = null;
        //    var result = validator.Validate(createEmail);
        //    Assert.IsFalse(result.IsValid);
        //}

        //[TestMethod]
        //public void MustHaveEmailTypeId()
        //{
        //    createEmail.Resource.EmailTypeId = null;
        //    var result = validator.Validate(createEmail);
        //    Assert.IsFalse(result.IsValid);
        //}
    }
}