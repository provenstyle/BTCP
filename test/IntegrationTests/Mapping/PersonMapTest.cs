namespace IntegrationTests.Mapping
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class PersonMapTest : BibleTrainingScenario
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
