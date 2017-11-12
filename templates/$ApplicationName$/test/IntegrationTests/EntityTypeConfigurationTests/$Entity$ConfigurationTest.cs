namespace IntegrationTests.EntityTypeConfigurationTests
{
    using $ApplicationName$.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class $Entity$EntityTypeConfigurationTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<$Entity$>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<$Entity$>();
        }
    }
}
