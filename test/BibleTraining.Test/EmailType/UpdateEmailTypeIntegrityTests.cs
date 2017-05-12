namespace BibleTraining.Test.EmailType
{
    using Api.EmailType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdateEmailTypeIntegrityTests
    {
        private UpdateEmailType _updateEmailType;
        private UpdateEmailTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _updateEmailType = new UpdateEmailType
            {
                Resource = new EmailTypeData
                {
                    Name = "my name"
                }
            };

            validator = new UpdateEmailTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(_updateEmailType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            _updateEmailType.Resource.Name = string.Empty;
            var result = validator.Validate(_updateEmailType);
            Assert.IsFalse(result.IsValid);
        }
    }
}