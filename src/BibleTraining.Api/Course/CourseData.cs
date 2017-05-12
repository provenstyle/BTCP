namespace BibleTraining.Api.Course
{
    using Improving.MediatR;

    public class CourseData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}