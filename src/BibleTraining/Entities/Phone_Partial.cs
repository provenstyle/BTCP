namespace BibleTraining.Entities
{

    public partial class Phone
    {
        public int PersonId { get; set; }

        public int       PhoneTypeId { get; set; }
        public PhoneType PhoneType   { get; set; }
    }
}
