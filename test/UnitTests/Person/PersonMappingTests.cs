namespace UnitTests.Person
{
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BibleTraining.Entities;
    using BibleTraining.Api.Person;
    using Miruken;
    using Miruken.Map;

    [TestClass]
    public class PersonMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<PersonData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = _handler.Proxy<IMapping>().Map<Person>(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.FirstName, entity.FirstName);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<Person>(new PersonData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<Person>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = _handler.Proxy<IMapping>().Map<PersonData>(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.FirstName,        resource.FirstName);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<PersonData>(new Person());
        }
    }
}
