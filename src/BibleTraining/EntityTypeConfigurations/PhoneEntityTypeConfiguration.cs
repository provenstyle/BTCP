namespace BibleTraining.EntityTypeConfigurations
{
    using Entities;

    public class PhoneEntityTypeConfiguration : BaseEntityTypeConfiguration<Phone>
    {
        public PhoneEntityTypeConfiguration()
        {
            ToTable(nameof(Phone));
        }
    }
}
