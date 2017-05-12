namespace BibleTraining.Api.AddressType
{
    using Api;

    public static class AddressTypeExtensions
    {
        public static AddressType Map(this AddressType addressType, AddressTypeData data)
        {
            EntityMapper.Map(addressType, data);

            if (data.Name != null)
                addressType.Name = data.Name;

            if (data.Description != null)
                addressType.Description = data.Description;

            return addressType;
        }
    }
}