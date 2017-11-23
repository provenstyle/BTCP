namespace BibleTraining.Entities
{
    public class Phone : Entity
    {
        public string    Number      { get; set; }
        public string    Extension   { get; set; }

        public int       PhoneTypeId { get; set; }
        public PhoneType PhoneType   { get; set; }

        public int       PersonId    { get; set; }
        public Person    Person      { get; set; }
    }
}
