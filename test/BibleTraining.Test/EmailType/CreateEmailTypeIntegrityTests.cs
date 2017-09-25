namespace BibleTraining.Test.EmailType 
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Api.EmailType;
    
    [TestClass]
    public class CreateEmailTypeIntegrityTests
    {
        private CreateEmailType createEmailType;
        private CreateEmailTypeIntegrity validator;

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

            validator = new CreateEmailTypeIntegrity();
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
