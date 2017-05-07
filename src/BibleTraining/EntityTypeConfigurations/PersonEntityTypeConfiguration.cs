namespace BibleTraining.EntityTypeConfigurations
{
    using Entities;

    public class PersonEntityTypeConfiguration : BaseEntityTypeConfiguration<Person>
    {
        public PersonEntityTypeConfiguration()
        {
            ToTable(nameof(Person));
        }
    }
}