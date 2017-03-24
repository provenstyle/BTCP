namespace BibleTraining.Maps
{
    using Entities;

    public class CourseMap : BaseMap<Course>
    {
        public CourseMap()
        {
            ToTable(nameof(Course));
        }
    }
}