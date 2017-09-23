namespace BibleTraining.Api.Address
{
    using AddressType;

    public class AddressData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }

        public int?   PersonId      { get; set; }

        public AddressTypeData AddressType { get; set; }
        public int?            AddressTypeId { get; set; }
    }
}