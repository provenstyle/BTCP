namespace BibleTraining.EntityTypeConfigurations
{
    using Api.AddressType;

    public class AddressTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<AddressType>
    {
        public AddressTypeEntityTypeConfiguration ()
        {
            ToTable(nameof(AddressType));
        }
    }
}