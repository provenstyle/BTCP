namespace UnitTests.EmailType
{
    using BibleTraining.Api.EmailType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdateEmailTypeIntegrityTests
    {
        private UpdateEmailType updateEmailType;
        private UpdateEmailTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updateEmailType =  new UpdateEmailType
            {
                 Resource = new EmailTypeData
                 {
                    Id   = 1,
                    Name = "a"
                 }
            };

            validator = new UpdateEmailTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updateEmailType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            updateEmailType.Resource.Name = string.Empty;
            var result = validator.Validate(updateEmailType);
            Assert.IsFalse(result.IsValid);
        }
    }
}
