namespace UnitTests.Person
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BibleTraining.Entities;
    using BibleTraining.Api.Person;
    using Miruken;
    using Miruken.Map;
    using Ploeh.AutoFixture;

    [TestClass]
    public class PersonMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Fixture.Create<PersonData>();
            var entity = _handler.Proxy<IMapping>().Map<Person>(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.FirstName, entity.FirstName);
            Assert.AreEqual(resource.LastName,  entity.LastName);

            //Assert.AreEqual(resource.Addresses.Count, entity.Addresses.Count);
            //Assert.AreEqual(resource.Emails.Count,    entity.Emails.Count);
            //Assert.AreEqual(resource.Phones.Count,    entity.Phones.Count);
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Fixture.Create<Person>();
            entity.Addresses = Fixture.Create<Address[]>();
            entity.Emails = Fixture.Create<Email[]>();
            entity.Phones = Fixture.Create<Phone[]>();

            var resource = _handler.Proxy<IMapping>().Map<PersonData>(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.FirstName,        resource.FirstName);
            Assert.AreEqual(entity.LastName,        resource.LastName);

            Assert.AreEqual(entity.Addresses.Count, resource.Addresses.Count);
            Assert.AreEqual(entity.Emails.Count,    resource.Emails.Count);
            Assert.AreEqual(entity.Phones.Count,    resource.Phones.Count);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<Person>(new PersonData());
        }


        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<PersonData>(new Person());
        }
    }
}
