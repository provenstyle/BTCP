namespace UnitTests.$Entity$ 
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using $ApplicationName$.Api.$Entity$;
    
    [TestClass]
    public class Create$Entity$IntegrityTests
    {
        private Create$Entity$ create$Entity$;
        private Create$Entity$Integrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            create$Entity$ =  new Create$Entity$
            {
                 Resource = new $Entity$Data
                 {
                    Name = "a"
                 }
            };

            validator = new Create$Entity$Integrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(create$Entity$);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveName()
        {
            create$Entity$.Resource.Name = string.Empty;
            var result = validator.Validate(create$Entity$);
            Assert.IsFalse(result.IsValid);
        }
    }
}
