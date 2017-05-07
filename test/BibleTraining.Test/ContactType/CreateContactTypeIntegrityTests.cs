namespace BibleTraining.Test.ContactType
{
    using Api.ContactType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateContactTypeIntegrityTests
    {
        private CreateContactType createContactType;
        private CreateContactTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createContactType = new CreateContactType
            {
                Resource = new ContactTypeData
                {
                    Name = "my name"
                }
            };

            validator = new CreateContactTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createContactType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveText()
        {
            createContactType.Resource.Name = string.Empty;
            var result = validator.Validate(createContactType);
            Assert.IsFalse(result.IsValid);
        }
    }
}