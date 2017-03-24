namespace BibleTraining.Test.Course
{
    using System.Data.Entity.Core;
    using System.Linq;
    using Api.Course;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Entities;
    using FizzWare.NBuilder;
    using Improving.MediatR;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class CourseConcurrencyTests : TestScenario
    {
        private Course _course;

        protected override void BeforeContainer(IWindsorContainer container)
        {
            _course = Builder<Course>.CreateNew()
                .With(b => b.Id = 1)
                .And(b => b.RowVersion = new byte[] { 0x02 })
                .Build();
            container.Register(Component.For<Course>().Instance(_course));
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnUpdate()
        {
            var course = Builder<CourseData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Course>())
                .Return(new[] { _course }.AsQueryable().TestAsync());

            var request = new UpdateCourse(course);

            try
            {
                AssertNoValidationErrors<CourseConcurency, UpdateResource<CourseData, int>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Course).FullName} with id 1.");
            }
        }

        [TestMethod]
        public void DetectsConcurrencyViolationOnRemove()
        {
            var course = Builder<CourseData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Course>())
                .Return(new[] { _course }.AsQueryable().TestAsync());

            var request = new RemoveCourse(course);

            try
            {
                AssertNoValidationErrors<CourseConcurency, UpdateResource<CourseData, int>>(request);
                Assert.Fail("Should have thrown OptimisticConcurrencyException");
            }
            catch (OptimisticConcurrencyException ex)
            {
                Assert.AreEqual(ex.Message,
                                $"Concurrency exception detected for {typeof(Course).FullName} with id 1.");
            }
        }
    }
}

