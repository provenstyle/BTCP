namespace BibleTraining.Test.Phone
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.Entity.Core;
    using System.Linq;
    using Api.Phone;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Entities;
    using FizzWare.NBuilder;
    using Improving.MediatR;
    using Infrastructure;
    using Rhino.Mocks;

    [TestClass]
    public class PhoneConcurrencyTests : TestScenario
    {
        private Phone _phone;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _phone = Builder<Phone>.CreateNew()
                .With(b => b.Id = 1)
                .And(b => b.RowVersion = new byte[] { 0x02 })
                .Build();
            container.Register(Component.For<Phone>().Instance(_phone));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var phone = Builder<PhoneData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Phone>())
                .Return(new[] { _phone }.AsQueryable().TestAsync());

            var request = new UpdatePhone(phone);

            try
            {
                AssertNoValidationErrors<PhoneConcurency, UpdateResource<PhoneData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Phone).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var phone = Builder<PhoneData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Phone>())
                .Return(new[] { _phone }.AsQueryable().TestAsync());

            var request = new RemovePhone(phone);

            try
            {
                AssertNoValidationErrors<PhoneConcurency, UpdateResource<PhoneData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Phone).FullName} with id 1.");
            }
        }
    }
}
