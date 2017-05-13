namespace BibleTraining.Api.Phone
{
    using Improving.MediatR;
    using PhoneType;

    public class PhoneData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }

        public int?          PersonId    { get; set; }

        public int?          PhoneTypeId { get; set; }
        public PhoneTypeData PhoneType   { get; set; }
    }
}