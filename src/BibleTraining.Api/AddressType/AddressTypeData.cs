namespace BibleTraining.Api.AddressType
{
    using Improving.MediatR;

    public class AddressTypeData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}