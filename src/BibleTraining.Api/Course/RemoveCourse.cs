namespace BibleTraining.Api.Course
{
    using Improving.MediatR;

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