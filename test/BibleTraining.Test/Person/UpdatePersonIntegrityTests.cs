namespace BibleTraining.Test.Person
{
    using Api.Person;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdatePersonIntegrityTests
    {
        private UpdatePerson updatePerson;
        private UpdatePersonIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updatePerson =  new UpdatePerson
            {
                Resource = new PersonData
                {
                    FirstName = "A",
                    LastName  = "B",
                    Gender    = Gender.Female
                }
            };

            validator = new UpdatePersonIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updatePerson);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveFirstName()
        {
            updatePerson.Resource.FirstName = string.Empty;
            var result = validator.Validate(updatePerson);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHaveLastName()
        {
            updatePerson.Resource.LastName = string.Empty;
            var result = validator.Validate(updatePerson);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHaveGender()
        {
            updatePerson.Resource.Gender = null;
            var result = validator.Validate(updatePerson);
            Assert.IsFalse(result.IsValid);
        }
    }
}