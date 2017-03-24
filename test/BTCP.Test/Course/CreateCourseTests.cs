namespace BibleTraining.Test.Course
{
    using System.Threading.Tasks;
    using Api.Course;
    using Entities;
    using FizzWare.NBuilder;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class CreateCourseTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldCreateCourse()
        {
            var course = Builder<CourseData>.CreateNew()
                .With(pg => pg.Id = 0).And(pg => pg.RowVersion = null)
                .Build();

            _context.Expect(pg => pg.Add(Arg<Course>.Is.Anything))
                .WhenCalled(inv =>
                    {
                        var entity = (Course)inv.Arguments[0];
                        entity.Id         = 1;
                        entity.RowVersion = new byte[] { 0x01 };
                        Assert.AreEqual(course.Description, entity.Description);
                        inv.ReturnValue = entity;
                    }).Return(null);

            _context.Expect(pg => pg.CommitAsync())
                .Return(Task.FromResult(1));

            var result = await _mediator.SendAsync(new CreateCourse(course));
            Assert.AreEqual(1, result.Id);
            CollectionAssert.AreEqual(new byte[] { 0x01 }, result.RowVersion);

            _context.VerifyAllExpectations();
        }
    }
}