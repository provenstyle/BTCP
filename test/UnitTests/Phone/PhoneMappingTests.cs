namespace UnitTests.Phone
{
    using BibleTraining.Api.Phone;
    using BibleTraining.Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Miruken;
    using Miruken.Map;

    [TestClass]
    public class PhoneMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<PhoneData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = _handler.Proxy<IMapping>().Map<Phone>(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Number,      entity.Number);
            Assert.AreEqual(resource.Extension,   entity.Extension);
            Assert.AreEqual(resource.PhoneTypeId, entity.PhoneTypeId);
            Assert.AreEqual(resource.PersonId,    entity.PersonId);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<Phone>(new PhoneData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<Phone>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = _handler.Proxy<IMapping>().Map<PhoneData>(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Number,      resource.Number);
            Assert.AreEqual(entity.Extension,   resource.Extension);
            Assert.AreEqual(entity.PhoneTypeId, resource.PhoneTypeId);
            Assert.AreEqual(entity.PersonId,    resource.PersonId);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<PhoneData>(new Phone());
        }
    }
}
