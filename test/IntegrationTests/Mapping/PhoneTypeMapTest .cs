namespace IntegrationTests.Mapping
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class PhoneTypeMapTest : BibleTrainingScenario
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
