namespace IntegrationTests.Mapping
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class CourseMapTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<Course>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<Course>();
        }
    }
}
