namespace UnitTests.Phone
{
    using BibleTraining.Api.Email;
    using BibleTraining.Api.Phone;
    using BibleTraining.Entities;
    using FluentValidation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken.Mediate;
    using Miruken.Validate.FluentValidation;

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
                    Name = "a",
                    PhoneTypeId = 1,
                    PersonId = 1
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
        public void MustHaveName()
        {
            createPhone.Resource.Name = string.Empty;
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
            var context = new ValidationContext<CreatePhone>(createPhone);
            context.SetComposer(stash);
            var result = validator.Validate(context);
            Assert.IsTrue(result.IsValid);
        }
    }
}
