namespace IntegrationTests.Mapping
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class AddressMapTest : BibleTrainingScenario
    {
        [TestMethod]
        public void CanCreate()
        {
            AssertCanCreateEntity<Address>();
        }

        [TestMethod]
        public void CanSelect()
        {
            AssertCanSelectTopOne<Address>();
        }
    }
}
