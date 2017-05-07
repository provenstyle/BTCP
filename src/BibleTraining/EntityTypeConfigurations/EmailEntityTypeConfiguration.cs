namespace BibleTraining.EntityTypeConfigurations
{
    using Entities;

    public class EmailEntityTypeConfiguration : BaseEntityTypeConfiguration<Email>
    {
        public EmailEntityTypeConfiguration()
        {
            ToTable(nameof(Email));
        }
    }
}