namespace IntegrationTests.Mapping
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class EmailMapTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<Email>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<Email>();
        }
    }
}
