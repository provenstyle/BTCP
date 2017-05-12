namespace BibleTraining.Api.Address
{
    using Improving.MediatR;

    public class AddressData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }

        public int?   PersonId    { get; set; }
    }
}