namespace BibleTraining.Entities
{
    using Api;

    public class Address : Entity, IKeyProperties<int>
    {
        public string Name               { get; set; }
        public string Description        { get; set; }

        public int         PersonId      { get; set; }
        public Person      Person        { get; set; }

        public int         AddressTypeId { get; set; }
        public AddressType AddressType   { get; set; }
    }
}