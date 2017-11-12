namespace BibleTraining.Entities
{
	using Api;

    public partial class Phone : Entity, IKeyProperties<int>
    {
        public string    Name        { get; set; }

        public int       PhoneTypeId { get; set; }
        public PhoneType PhoneType   { get; set; }

        public int       PersonId    { get; set; }
        public Person    Person      { get; set; }
    }
}
