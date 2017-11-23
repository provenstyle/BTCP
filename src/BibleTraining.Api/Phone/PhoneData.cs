namespace BibleTraining.Api.Phone
{
    using Person;
    using PhoneType;

    public class PhoneData : Resource<int?>
    {
        public string Number             { get; set; }
        public string Extension          { get; set; }

        public int?       PersonId       { get; set; }
        public PersonData Person         { get; set; }

        public int?          PhoneTypeId { get; set; }
        public PhoneTypeData PhoneType   { get; set; }
    }
}
