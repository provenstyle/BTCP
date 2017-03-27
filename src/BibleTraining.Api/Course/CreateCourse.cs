namespace BibleTraining.Api.Course
{
    using Improving.MediatR;

    public class CreateCourse : ResourceAction<CourseData, int>
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