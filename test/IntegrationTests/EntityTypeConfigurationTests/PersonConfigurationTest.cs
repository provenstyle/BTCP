namespace IntegrationTests.EntityTypeConfigurationTests
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class PersonEntityTypeConfigurationTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<Person>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<Person>();
        }
    }
}
