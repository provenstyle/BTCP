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
    public class UpdateCourseTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldUpdateCourse()
        {
            var course= new Course()
            {
                Id         = 1,
                Name       = "ABC",
                RowVersion = new byte[] { 0x01 }
            };

            var courseData = Builder<CourseData>.CreateNew()
                .With(c => c.Id = 1).And(c => c.RowVersion = new byte[] { 0x01 })
                .Build();

            _context.Expect(c => c.AsQueryable<Course>())
                .Return(new[] { course }.AsQueryable().TestAsync());

            _context.Expect(c => c.CommitAsync())
                .WhenCalled(inv => course.RowVersion = new byte[] { 0x02 })
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new UpdateCourse(courseData));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x02 }, result.RowVersion);

            Assert.AreEqual(courseData.Name, course.Name);

            _context.VerifyAllExpectations();
        }
    }
}