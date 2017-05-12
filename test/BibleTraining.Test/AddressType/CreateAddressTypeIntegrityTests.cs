namespace BibleTraining.Test.AddressType
{
    using Api.AddressType;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateAddressTypeIntegrityTests
    {
        private CreateAddressType createAddressType;
        private CreateAddressTypeIntegrity validator;

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

            validator = new CreateAddressTypeIntegrity();
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