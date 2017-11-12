namespace BibleTraining.Test.Course
{
    using Api.Course;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UpdateCourseIntegrityTests
    {
        private UpdateCourse updateCourse;
        private UpdateCourseIntegrity validator;

        [TestInitialize]
        public void TestInitialize()
        {
            updateCourse =  new UpdateCourse
            {
                Resource = new CourseData
                {
                    Description = "my text"
                }
            };

            validator = new UpdateCourseIntegrity();
        }

        [TestMethod]
        public void IsValid()
        {
            var result = validator.Validate(updateCourse);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MustHaveText()
        {
            updateCourse.Resource.Description = string.Empty;
            var result = validator.Validate(updateCourse);
            Assert.IsFalse(result.IsValid);
        }
    }
}