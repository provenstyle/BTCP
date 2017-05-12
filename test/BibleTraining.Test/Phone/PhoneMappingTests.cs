namespace BibleTraining.Test.Phone
{
    using Api.Phone;
    using Entities;
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PhoneMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<PhoneData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = new Phone().Map(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Name, entity.Name);
            Assert.AreEqual(resource.Description, entity.Description);

            Assert.AreEqual(resource.PersonId, entity.PersonId);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            new Phone().Map(new PhoneData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<Phone>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = new PhoneData().Map(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Name,        resource.Name);
            Assert.AreEqual(entity.Description, resource.Description);

            Assert.AreEqual(entity.PersonId, resource.PersonId);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            new PhoneData().Map(new Phone());
        }
    }
}