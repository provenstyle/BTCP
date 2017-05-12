namespace BibleTraining.Test.Address
{
    using Api.Address;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using _CodeGeneration;

    [TestClass]
    public class CreateAddressIntegrityTests
    {
        private CreateAddress createAddress;
        private CreateAddressIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createAddress =  new CreateAddress
            {
                Resource = new AddressData
                {
                    PersonId = 1,
                    Name     = "a"
                }
            };

            validator = new CreateAddressIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createAddress);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            createAddress.Resource.Name = string.Empty;
            var result = validator.Validate(createAddress);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHavePersonId()
        {
            createAddress.Resource.PersonId = null;
            var result = validator.Validate(createAddress);
            Assert.IsFalse(result.IsValid);
        }
    }
}