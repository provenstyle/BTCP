namespace BibleTraining.Test.Email
{
    using System.Data.Entity.Core;
    using System.Linq;
    using Api.Email;
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
    public class EmailConcurrencyTests : TestScenario
    {
        private Email _email;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _email = Builder<Email>.CreateNew()
                .With(b => b.Id = 1)
                .And(b => b.RowVersion = new byte[] { 0x02 })
                .Build();
            container.Register(Component.For<Email>().Instance(_email));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var email = Builder<EmailData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Email>())
                .Return(new[] { _email }.AsQueryable().TestAsync());

            var request = new UpdateEmail(email);

            try
            {
                AssertNoValidationErrors<EmailConcurency, UpdateResource<EmailData, int>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Email).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var email = Builder<EmailData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Email>())
                .Return(new[] { _email }.AsQueryable().TestAsync());

            var request = new RemoveEmail(email);

            try
            {
                AssertNoValidationErrors<EmailConcurency, UpdateResource<EmailData, int>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Email).FullName} with id 1.");
            }
        }
    }
}
