namespace BibleTraining.Test.Course
{
    using Api.Course;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateCourseIntegrityTests
    {
        private CreateCourse createCourse;
        private CreateCourseIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            createCourse =  new CreateCourse
            {
                Resource = new CourseData
                {
                    Description = "my text"
                }
            };

            validator = new CreateCourseIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(createCourse);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveDescription()
        {
            createCourse.Resource.Description = string.Empty;
            var result = validator.Validate(createCourse);
            Assert.IsFalse(result.IsValid);
        }
    }
}