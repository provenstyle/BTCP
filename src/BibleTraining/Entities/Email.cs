namespace BibleTraining.Entities
{
    public class Email : Entity
    {
        public string    Address     { get; set; }

        public EmailType EmailType   { get; set; }
        public int       EmailTypeId { get; set; }

        public Person    Person      { get; set; }
        public int       PersonId    { get; set; }
    }
}