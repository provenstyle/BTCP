namespace UnitTests.Phone
{
    using BibleTraining.Api.Phone;
    using BibleTraining.Entities;
    using FluentValidation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Miruken.Validate.FluentValidation;

    [TestClass]
    public class CreateUpdatePhoneIntegrityTests
    {
        private CreatePhone createPhone;
        private CreateUpdatePhoneIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createPhone =  new CreatePhone
            {
                 Resource = new PhoneData
                 {
                    Number = "1 940 395 5555",
                    PhoneTypeId = 1,
                    PersonId = 1,
                 }
            };

            validator = new CreateUpdatePhoneIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createPhone);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveNumber()
        {
            createPhone.Resource.Number = string.Empty;
            var result = validator.Validate(createPhone);
            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void NumberMustNotBeAString()
        {
            createPhone.Resource.Number = "a";
            var result = validator.Validate(createPhone);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void NumberMustBeValid()
        {
            createPhone.Resource.Number = "395 555";
            var result = validator.Validate(createPhone);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHavePersonId()
        {
            createPhone.Resource.PersonId = null;
            var result = validator.Validate(createPhone);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHavePersonInStash()
        {
            var stash = new Stash();
            stash.Put(new Person { Id = 1});
            createPhone.Resource.PersonId = null;
            var context = new ValidationContext<IValidateCreateUpdatePhone>(createPhone);
            context.SetComposer(stash);
            var result = validator.Validate(context);
            Assert.IsTrue(result.IsValid);
        }
    }
}
