namespace BibleTraining.Api.PhoneType
{
    using Improving.MediatR;

    public class PhoneTypeData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}
