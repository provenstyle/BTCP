namespace BibleTraining.EntityTypeConfigurations
{
    using Entities;

    public class AddressEntityTypeConfiguration : BaseEntityTypeConfiguration<Address>
    {
        public AddressEntityTypeConfiguration ()
        {
            ToTable(nameof(Address));
        }
    }
}