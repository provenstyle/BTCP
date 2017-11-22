namespace UnitTests.EmailType
{
    using BibleTraining.Api.EmailType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateUpdateEmailTypeIntegrityTests
    {
        private CreateEmailType createEmailType;
        private CreateUpdateEmailTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createEmailType =  new CreateEmailType
            {
                 Resource = new EmailTypeData
                 {
                    Name = "a"
                 }
            };

            validator = new CreateUpdateEmailTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createEmailType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            createEmailType.Resource.Name = string.Empty;
            var result = validator.Validate(createEmailType);
            Assert.IsFalse(result.IsValid);
        }
    }
}
