namespace BibleTraining.Test.EmailType
{
    using Api.EmailType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateEmailTypeIntegrityTests
    {
        private CreateEmailType _createEmailType;
        private CreateEmailTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _createEmailType = new CreateEmailType
            {
                Resource = new EmailTypeData
                {
                    Name = "my name"
                }
            };

            validator = new CreateEmailTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(_createEmailType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveText()
        {
            _createEmailType.Resource.Name = string.Empty;
            var result = validator.Validate(_createEmailType);
            Assert.IsFalse(result.IsValid);
        }
    }
}