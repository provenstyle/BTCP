namespace BibleTraining.Test.Address
{
    using System.Data.Entity.Core;
    using System.Linq;
    using Api;
    using Api.Address;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class AddressConcurrencyTests : TestScenario
    {
        private Address _address;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _address = Builder<Address>.CreateNew()
                .With(b => b.Id = 1)
                .And(b => b.RowVersion = new byte[] { 0x02 })
                .Build();
            container.Register(Component.For<Address>().Instance(_address));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var address = Builder<AddressData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Address>())
                .Return(new[] { _address }.AsQueryable().TestAsync());

            var request = new UpdateAddress(address);

            try
            {
                AssertNoValidationErrors<AddressConcurency, UpdateResource<AddressData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Address).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var address = Builder<AddressData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Address>())
                .Return(new[] { _address }.AsQueryable().TestAsync());

            var request = new RemoveAddress(address);

            try
            {
                AssertNoValidationErrors<AddressConcurency, UpdateResource<AddressData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Address).FullName} with id 1.");
            }
        }
    }
}
