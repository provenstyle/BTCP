namespace BibleTraining.EntityTypeConfigurations
{
    using Entities;

    public class ContactTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<EmailType>
    {
        public ContactTypeEntityTypeConfiguration()
        {
            ToTable(nameof(EmailType));
        }
    }
}