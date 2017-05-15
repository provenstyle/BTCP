namespace BibleTraining.Api.AddressType
{
    using Api;
    using Entities;

    public static class AddressTypeDataExtensions
    {
        public static AddressTypeData Map(this AddressTypeData data, AddressType addressType)
        {
            if (addressType == null) return null;

            ResourceMapper.Map(data, addressType);

            data.Name        = addressType.Name;
            data.Description = addressType.Description;

            return data;
        }
    }
}