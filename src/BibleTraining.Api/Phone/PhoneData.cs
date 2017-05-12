namespace BibleTraining.Api.Phone
{
    using Improving.MediatR;

    public class PhoneData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }

        public int?   PersonId    { get; set; }
    }
}