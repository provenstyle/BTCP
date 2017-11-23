namespace UnitTests.PhoneType
{
    using System.Data.Entity.Core;
    using System.Linq;
    using BibleTraining.Api;
    using BibleTraining.Api.PhoneType;
    using BibleTraining.Entities;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class PhoneTypeConcurrencyTests : TestScenario
    {
        private PhoneType _phoneType;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _phoneType = Builder<PhoneType>.CreateNew()
                 .With(b => b.Id = 1)
                 .And(b => b.RowVersion = new byte[] { 0x02 })
                 .Build();
            container.Register(Component.For<PhoneType>().Instance(_phoneType));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var phoneType = Builder<PhoneTypeData>.CreateNew()
               .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
               .Build();

            _context.Expect(c => c.AsQueryable<PhoneType>())
                .Return(new[] { _phoneType }.AsQueryable().TestAsync());

            var request = new UpdatePhoneType(phoneType);

            try
            {
                AssertNoValidationErrors<PhoneTypeConcurency, UpdateResource<PhoneTypeData, int?>, PhoneType>(request, _phoneType);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                    $"Concurrency exception detected for {typeof(PhoneType).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var phoneType = Builder<PhoneTypeData>.CreateNew()
               .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
               .Build();

            _context.Expect(c => c.AsQueryable<PhoneType>())
                .Return(new[] { _phoneType }.AsQueryable().TestAsync());

            var request = new RemovePhoneType(phoneType);

            try
            {
                AssertNoValidationErrors<PhoneTypeConcurency, UpdateResource<PhoneTypeData, int?>, PhoneType>(request, _phoneType);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                    $"Concurrency exception detected for {typeof(PhoneType).FullName} with id 1.");
            }
        }
    }
}
