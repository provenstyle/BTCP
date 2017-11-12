namespace UnitTests.$Entity$
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Api.$Entity$;
    
    [TestClass]
    public class Update$Entity$IntegrityTests
    {
        private Update$Entity$ update$Entity$;
        private Update$Entity$Integrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            update$Entity$ =  new Update$Entity$
            {
                 Resource = new $Entity$Data
                 {
                    Id   = 1,
                    Name = "a"
                 }
            };

            validator = new Update$Entity$Integrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(update$Entity$);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            update$Entity$.Resource.Name = string.Empty;
            var result = validator.Validate(update$Entity$);
            Assert.IsFalse(result.IsValid);
        }
    }
}
