namespace IntegrationTests.EntityTypeConfigurationTests
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class PhoneTypeEntityTypeConfigurationTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<PhoneType>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<PhoneType>();
        }
    }
}
