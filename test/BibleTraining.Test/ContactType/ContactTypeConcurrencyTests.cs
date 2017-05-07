namespace BibleTraining.Test.ContactType
{
    using System.Data.Entity.Core;
    using System.Linq;
    using Api.ContactType;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Entities;
    using FizzWare.NBuilder;
    using Improving.MediatR;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Test;

    [TestClass]
    public class ContactTypeConcurrencyTests : TestScenario
    {
        private EmailType _emailType;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _emailType = Builder<EmailType>.CreateNew()
                .With(b => b.Id = 1)
                .And(b => b.RowVersion = new byte[] { 0x02 })
                .Build();
            container.Register(Component.For<EmailType>().Instance(_emailType));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var contactType = Builder<ContactTypeData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<EmailType>())
                .Return(new[] { _emailType }.AsQueryable().TestAsync());

            var request = new UpdateContactType(contactType);

            try
            {
                AssertNoValidationErrors<ContactTypeConcurency, UpdateResource<ContactTypeData, int>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(EmailType).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var contactType = Builder<ContactTypeData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<EmailType>())
                .Return(new[] { _emailType }.AsQueryable().TestAsync());

            var request = new RemoveContactType(contactType);

            try
            {
                AssertNoValidationErrors<ContactTypeConcurency, UpdateResource<ContactTypeData, int>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(EmailType).FullName} with id 1.");
            }
        }
    }
}
