namespace BibleTraining.Api.Course
{
    public class RemoveCourse : UpdateResource<CourseData, int?>
    {
        public RemoveCourse()
        {
        }

        public RemoveCourse(CourseData course)
            : base(course)
        {
        }
    }
}