namespace UnitTests.Person
{
    using System.Data.Entity.Core;
    using System.Linq;
    using BibleTraining.Api;
    using BibleTraining.Api.Person;
    using BibleTraining.Entities;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using UnitTests;

    [TestClass]
    public class PersonConcurrencyTests : TestScenario
    {
        private Person _person;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _person = Builder<Person>.CreateNew()
                .With(b => b.Id = 1)
                .And(b => b.RowVersion = new byte[] { 0x02 })
                .Build();
            container.Register(Component.For<Person>().Instance(_person));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var person = Builder<PersonData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Person>())
                .Return(new[] { _person }.AsQueryable().TestAsync());

            var request = new UpdatePerson(person);

            try
            {
                AssertNoValidationErrors<PersonConcurency, UpdateResource<PersonData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Person).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var person = Builder<PersonData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Person>())
                .Return(new[] { _person }.AsQueryable().TestAsync());

            var request = new RemovePerson(person);

            try
            {
                AssertNoValidationErrors<PersonConcurency, UpdateResource<PersonData, int?>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Person).FullName} with id 1.");
            }
        }
    }
}