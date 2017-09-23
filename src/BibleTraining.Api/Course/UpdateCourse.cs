namespace BibleTraining.Api.Course
{
    public class UpdateCourse : UpdateResource<CourseData, int?>
    {
        public UpdateCourse()
        {
        }

        public UpdateCourse(CourseData course)
            : base(course)
        {
        }
    }
}