namespace IntegrationTests.Mapping
{
    using BibleTraining.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scenarios;

    [TestClass]
    public class AddressTypeMapTest : BibleTrainingScenario
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
