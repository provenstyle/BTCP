namespace BibleTraining.Api.Phone
{
    using PhoneType;

    public class PhoneData : Resource<int?>
    {
        public string        Number      { get; set; }
        public string        Extension   { get; set; }

        public int?          PersonId    { get; set; }

        public int?          PhoneTypeId { get; set; }
        public PhoneTypeData PhoneType   { get; set; }
    }
}
