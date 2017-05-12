namespace BibleTraining.Api.Course
{
    using Improving.MediatR;

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