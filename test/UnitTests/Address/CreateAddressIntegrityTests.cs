namespace UnitTests.Address
{
    using BibleTraining.Api.Address;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                    PersonId      = 1,
                    AddressTypeId = 1,
                    Name          = "a"
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

        //[TestMethod]
        //public void MustHavePersonId()
        //{
        //    createAddress.Resource.PersonId = null;
        //    var result = validator.Validate(createAddress);
        //    Assert.IsFalse(result.IsValid);
        //}

        //[TestMethod]
        //public void MustHaveAddressTypeId()
        //{
        //    createAddress.Resource.AddressTypeId = null;
        //    var result = validator.Validate(createAddress);
        //    Assert.IsFalse(result.IsValid);
        //}
    }
}