namespace BibleTraining.Test.Course
{
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Course;
    using Entities;
    using Infrastructure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    [TestClass]
    public class GetCoursesTests : TestScenario
    {
        [TestMethod]
        public async Task ShouldGetCourses()
        {
            SetupChoices();

            var result = await _mediator.SendAsync(new GetCourses());
            Assert.AreEqual(3, result.Courses.Length);

            _context.VerifyAllExpectations();
        }

        [TestMethod]
        public async Task ShouldGetOnlyKeyProperties()
        {
            _context.Stub(p => p.AsQueryable<Course>())
                .Return(TestChoice<Course>(3).TestAsync());

            var result = await _mediator.SendAsync(new GetCourses { KeyProperties = true });

            Assert.IsTrue(result.Courses.All(x => x.Name != null));
            Assert.IsTrue(result.Courses.All(x => x.CreatedBy == null));

            _context.VerifyAllExpectations();
        }
    }
}