namespace IntegrationTests.EntityTypeConfigurationTests
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class AddressTypeEntityTypeConfigurationTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<AddressType>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<AddressType>();
        }
    }
}
