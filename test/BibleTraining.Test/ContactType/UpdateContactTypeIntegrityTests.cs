namespace BibleTraining.Test.ContactType
{
    using Api.ContactType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdateContactTypeIntegrityTests
    {
        private UpdateContactType updateContactType;
        private UpdateContactTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updateContactType = new UpdateContactType
            {
                Resource = new ContactTypeData
                {
                    Name = "my name"
                }
            };

            validator = new UpdateContactTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updateContactType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            updateContactType.Resource.Name = string.Empty;
            var result = validator.Validate(updateContactType);
            Assert.IsFalse(result.IsValid);
        }
    }
}