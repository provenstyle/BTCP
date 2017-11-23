namespace UnitTests.AddressType 
{
    using BibleTraining.Api.AddressType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateUpdateAddressTypeIntegrityTests
    {
        private CreateAddressType createAddressType;
        private CreateUpdateAddressTypeIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createAddressType =  new CreateAddressType
            {
                 Resource = new AddressTypeData
                 {
                    Name = "a"
                 }
            };

            validator = new CreateUpdateAddressTypeIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createAddressType);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            createAddressType.Resource.Name = string.Empty;
            var result = validator.Validate(createAddressType);
            Assert.IsFalse(result.IsValid);
        }
    }
}
