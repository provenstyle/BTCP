namespace BibleTraining.Test.Address
{
    using Api.Address;
    using Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken;
    using Miruken.Map;

    [TestClass]
    public class AddressMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<AddressData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = _handler.Proxy<IMapping>().Map<Address>(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Name, entity.Name);
            Assert.AreEqual(resource.Description, entity.Description);

            Assert.AreEqual(resource.PersonId, entity.PersonId);
            Assert.AreEqual(resource.AddressTypeId, entity.AddressTypeId);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<Address>(new AddressData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<Address>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = _handler.Proxy<IMapping>().Map<AddressData>(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Name,        resource.Name);
            Assert.AreEqual(entity.Description, resource.Description);

            Assert.AreEqual(entity.PersonId, resource.PersonId);
            Assert.AreEqual(entity.AddressTypeId, resource.AddressTypeId);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<AddressData>(new Address());
        }
    }
}