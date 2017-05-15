namespace BibleTraining.Test.AddressType
{
    using Api.AddressType;
    using Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AddressTypeMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<AddressTypeData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = new AddressType().Map(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Name, entity.Name);
            Assert.AreEqual(resource.Description, entity.Description);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            new AddressType().Map(new AddressTypeData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<AddressType>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = new AddressTypeData().Map(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Name,        resource.Name);
            Assert.AreEqual(entity.Description, resource.Description);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            new AddressTypeData().Map(new AddressType());
        }
    }
}