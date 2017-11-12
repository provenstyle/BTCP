namespace IntegrationTests.EntityTypeConfigurationTests
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class EmailTypeEntityTypeConfigurationTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<EmailType>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<EmailType>();
        }
    }
}
