namespace UnitTests.AddressType
{
    using System.Data.Entity.Core;
    using System.Linq;
    using BibleTraining.Api;
    using BibleTraining.Api.AddressType;
    using BibleTraining.Entities;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class AddressTypeConcurrencyTests : TestScenario
    {
        private AddressType _addressType;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _addressType = Builder<AddressType>.CreateNew()
                 .With(b => b.Id = 1)
                 .And(b => b.RowVersion = new byte[] { 0x02 })
                 .Build();
            container.Register(Component.For<AddressType>().Instance(_addressType));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var addressType = Builder<AddressTypeData>.CreateNew()
               .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
               .Build();

            _context.Expect(c => c.AsQueryable<AddressType>())
                .Return(new[] { _addressType }.AsQueryable().TestAsync());

            var request = new UpdateAddressType(addressType);

            try
            {
                AssertNoValidationErrors<AddressTypeConcurency, UpdateResource<AddressTypeData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                    $"Concurrency exception detected for {typeof(AddressType).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var addressType = Builder<AddressTypeData>.CreateNew()
               .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
               .Build();

            _context.Expect(c => c.AsQueryable<AddressType>())
                .Return(new[] { _addressType }.AsQueryable().TestAsync());

            var request = new RemoveAddressType(addressType);

            try
            {
                AssertNoValidationErrors<AddressTypeConcurency, UpdateResource<AddressTypeData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                    $"Concurrency exception detected for {typeof(AddressType).FullName} with id 1.");
            }
        }
    }
}
