namespace BibleTraining.Test.PhoneType
{
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Entities;
    using Api.PhoneType;
    using Miruken;
    using Miruken.Map;

    [TestClass]
    public class PhoneTypeMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<PhoneTypeData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = _handler.Proxy<IMapping>().Map<PhoneType>(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Name, entity.Name);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<PhoneType>(new PhoneTypeData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<PhoneType>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = _handler.Proxy<IMapping>().Map<PhoneTypeData>(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Name,        resource.Name);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<PhoneTypeData>(new PhoneType());
        }
    }
}
