namespace BibleTraining.Test.AddressType
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Api.AddressType;
    
    [TestClass]
    public class UpdateAddressTypeIntegrityTests
    {
        private UpdateAddressType updateAddressType;
        private UpdateAddressTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updateAddressType =  new UpdateAddressType
            {
                 Resource = new AddressTypeData
                 {
                    Id   = 1,
                    Name = "a"
                 }
            };

            validator = new UpdateAddressTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updateAddressType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            updateAddressType.Resource.Name = string.Empty;
            var result = validator.Validate(updateAddressType);
            Assert.IsFalse(result.IsValid);
        }
    }
}
