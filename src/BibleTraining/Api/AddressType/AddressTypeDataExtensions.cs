namespace BibleTraining.Api.AddressType
{
    using Api;

    public static class AddressTypeDataExtensions
    {
        public static AddressTypeData Map(this AddressTypeData data, AddressType addressType)
        {
            ResourceMapper.Map(data, addressType);

            data.Name        = addressType.Name;
            data.Description = addressType.Description;

            return data;
        }
    }
}