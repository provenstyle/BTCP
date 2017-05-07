namespace BibleTraining.EntityTypeConfigurations
{
    using Entities;

    public class CourseEntityTypeConfiguration : BaseEntityTypeConfiguration<Course>
    {
        public CourseEntityTypeConfiguration()
        {
            ToTable(nameof(Course));
        }
    }
}