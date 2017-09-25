namespace BibleTraining.EntityTypeConfigurations
{
	using Entities;

    public class PhoneTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<PhoneType>
    {
        public PhoneTypeEntityTypeConfiguration()
        {
            ToTable(nameof(PhoneType));
        }
    }
}
