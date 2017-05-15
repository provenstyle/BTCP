namespace $ApplicationName$.Test.$Entity$
{
    using FizzWare.NBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class $Entity$MappingTests : TestScenario
    {
        [TestMethod]
        public void ShouldMapResourcesToEntities()
        {
            var resource = Builder<$Entity$Data>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var entity = new $Entity$().Map(resource);

            AssertResourcesMapToEntities(entity, resource);

            Assert.AreEqual(resource.Name, entity.Name);
            Assert.AreEqual(resource.Description, entity.Description);
        }

        [TestMethod]
        public void ShouldMapDefaultResourcesToEntitiesWithNoErrors()
        {
            new $Entity$().Map(new $Entity$Data());
        }

        [TestMethod]
        public void ShouldMapEntitiesToResources()
        {
            var entity = Builder<$Entity$>.CreateNew()
                .With(c => c.RowVersion = new byte[] { 0x01 })
                .Build();
            var resource = new $Entity$Data().Map(entity);

            AssertEntitiesMapToResources(resource, entity);

            Assert.AreEqual(entity.Name,        resource.Name);
            Assert.AreEqual(entity.Description, resource.Description);
        }

        [TestMethod]
        public void ShouldMapDefaultEntitiesToResourcesWithNoErrors()
        {
            new $Entity$Data().Map(new $Entity$());
        }
    }
}
