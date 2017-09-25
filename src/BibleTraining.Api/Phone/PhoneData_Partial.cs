namespace BibleTraining.Api.Phone
{
    using PhoneType;

    public partial class PhoneData
    {
        public int? PersonId { get; set; }

        public int?          PhoneTypeId { get; set; }
        public PhoneTypeData PhoneType   { get; set; }
    }
}
