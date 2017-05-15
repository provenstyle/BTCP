namespace BibleTraining.EntityTypeConfigurations
{
    using Entities;

    public class AddressTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<AddressType>
    {
        public AddressTypeEntityTypeConfiguration ()
        {
            ToTable(nameof(AddressType));
        }
    }
}