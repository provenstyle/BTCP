namespace BibleTraining.Test.Course
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Course;
    using Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class RemoveCourseTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldRemoveCourse()
        {
            var entity = new Course
            {
                Id          = 1,
                Description = "ABC",
                RowVersion  = new byte[] { 0x01 }
            };

            var courseData = Builder<CourseData>.CreateNew()
                .With(pg => pg.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(pg => pg.AsQueryable<Course>())
                .Return(new[] { entity }.AsQueryable().TestAsync());

            _context.Expect(c => c.Remove(entity))
                .Return(entity);

            _context.Expect(c => c.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new RemoveCourse(courseData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}