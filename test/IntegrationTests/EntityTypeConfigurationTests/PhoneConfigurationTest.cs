namespace IntegrationTests.EntityTypeConfigurationTests
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class PhoneEntityTypeConfigurationTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<Phone>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<Phone>();
        }
    }
}
