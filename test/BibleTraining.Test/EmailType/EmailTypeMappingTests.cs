namespace BibleTraining.Test.EmailType
{
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Entities;
    using Api.EmailType;
    using Miruken;
    using Miruken.Map;

    [TestClass]
    public class EmailTypeMappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<EmailTypeData>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = _handler.Proxy<IMapping>().Map<EmailType>(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Name, entity.Name);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<EmailType>(new EmailTypeData());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<EmailType>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = _handler.Proxy<IMapping>().Map<EmailTypeData>(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Name,        resource.Name);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            _handler.Proxy<IMapping>().Map<EmailTypeData>(new EmailType());
        }
    }
}
