namespace $ApplicationName$.Test.$Entity$
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.Entity.Core;
    using System.Linq;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using FizzWare.NBuilder;
    using Improving.MediatR;
    using Rhino.Mocks;

    [TestClass]
    public class $Entity$ConcurrencyTests : TestScenario
    {
        private $Entity$ _$entityLowercase$;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _$entityLowercase$ = Builder<$Entity$>.CreateNew()
                 .With(b => b.Id = 1)
                 .And(b => b.RowVersion = new byte[] { 0x02 })
                 .Build();
            container.Register(Component.For<$Entity$>().Instance(_$entityLowercase$));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var $entityLowercase$ = Builder<$Entity$Data>.CreateNew()
               .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
               .Build();

            _context.Expect(c => c.AsQueryable<$Entity$>())
                .Return(new[] { _$entityLowercase$ }.AsQueryable().TestAsync());

            var request = new Update$Entity$($entityLowercase$);

            try
            {
                AssertNoValidationErrors<$Entity$Concurency, UpdateResource<$Entity$Data, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                    $"Concurrency exception detected for {typeof($Entity$).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var $entityLowercase$ = Builder<$Entity$Data>.CreateNew()
               .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
               .Build();

            _context.Expect(c => c.AsQueryable<$Entity$>())
                .Return(new[] { _$entityLowercase$ }.AsQueryable().TestAsync());

            var request = new Remove$Entity$($entityLowercase$);

            try
            {
                AssertNoValidationErrors<$Entity$Concurency, UpdateResource<$Entity$Data, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                    $"Concurrency exception detected for {typeof($Entity$).FullName} with id 1.");
            }
        }
    }
}
