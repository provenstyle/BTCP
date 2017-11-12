namespace UnitTests.Email
{
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BibleTraining.Entities;
    using BibleTraining.Api.Email;
    using Miruken;
    using Miruken.Map;

    [TestClass]
    public class EmailMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<EmailData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = _handler.Proxy<IMapping>().Map<Email>(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Address, entity.Address);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<Email>(new EmailData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<Email>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = _handler.Proxy<IMapping>().Map<EmailData>(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Address,        resource.Address);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<EmailData>(new Email());
        }
    }
}
