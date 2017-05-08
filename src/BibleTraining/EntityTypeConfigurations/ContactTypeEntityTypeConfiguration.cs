namespace BibleTraining.EntityTypeConfigurations
{
    using Entities;

    public class EmailTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<EmailType>
    {
        public EmailTypeEntityTypeConfiguration()
        {
            ToTable(nameof(EmailType));
        }
    }
}