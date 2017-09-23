namespace BibleTraining.Api.Course
{
    public class CreateCourse : ResourceAction<CourseData, int?>
    {
        public CreateCourse()
        {
        }

        public CreateCourse(CourseData course)
            : base (course)
        {
        }
    }
}