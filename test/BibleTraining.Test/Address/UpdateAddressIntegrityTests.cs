namespace BibleTraining.Test.Address
{
    using Api.Address;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using _CodeGeneration;

    [TestClass]
    public class UpdateAddressIntegrityTests
    {
        private UpdateAddress updateAddress;
        private UpdateAddressIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updateAddress =  new UpdateAddress
            {
                Resource = new AddressData
                {
                    Id            = 1,
                    PersonId      = 1,
                    AddressTypeId = 1,
                    Name          = "a"
                }
            };

            validator = new UpdateAddressIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updateAddress);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            updateAddress.Resource.Name = string.Empty;
            var result = validator.Validate(updateAddress);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MustHavePersonId()
        {
            updateAddress.Resource.PersonId = null;
            var result = validator.Validate(updateAddress);
            Assert.IsFalse(result.IsValid);
        }
    }
}