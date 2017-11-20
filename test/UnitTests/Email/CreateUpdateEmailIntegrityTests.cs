namespace UnitTests.Email
{
    using BibleTraining.Api.Email;
    using BibleTraining.Entities;
    using FluentValidation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Miruken.Validate.FluentValidation;

    [TestClass]
    public class CreateUpdateEmailIntegrityTests
    {
        private CreateEmail createEmail;
        private CreateUpdateEmailIntegrity validator;

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

            validator = new CreateUpdateEmailIntegrity();
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

        [TestMethod]
        public void MustHaveEmailTypeId()
        {
            createEmail.Resource.EmailTypeId = null;
            var result = validator.Validate(createEmail);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHavePersonId()
        {
            createEmail.Resource.PersonId = null;
            var result = validator.Validate(createEmail);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHavePersonInStash()
        {
            var stash = new Stash();
            stash.Put(new Person { Id = 1});
            createEmail.Resource.PersonId = null;
            var context = new ValidationContext<IValidateCreateUpdateEmail>(createEmail);
            context.SetComposer(stash);
            var result = validator.Validate(context);
            Assert.IsTrue(result.IsValid);
        }
    }
}