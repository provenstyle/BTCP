namespace BibleTraining.Api.AddressType
{
    public class AddressTypeData : Resource<int?>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}