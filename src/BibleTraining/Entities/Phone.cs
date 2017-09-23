namespace BibleTraining.Entities
{
    using Api;

    public class Phone : Entity, IKeyProperties<int>
    {
        public string Name        { get; set; }
        public string Description { get; set; }

        public int       PersonId    { get; set; }

        public int       PhoneTypeId { get; set; }
        public PhoneType PhoneType   { get; set; }
    }
}