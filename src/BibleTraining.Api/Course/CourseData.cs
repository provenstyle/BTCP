namespace BibleTraining.Api.Course
{
    public class CourseData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}