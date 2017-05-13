namespace BibleTraining.Api.Address
{
    using AddressType;
    using Api;
    using Entities;

    public static class AddressDataExtensions
    {
        public static AddressData Map(this AddressData data, Address address)
        {
            if (address == null) return null;

            ResourceMapper.Map(data, address);

            data.Name          = address.Name;
            data.Description   = address.Description;

            data.PersonId      = address.PersonId;

            data.AddressTypeId = address.AddressTypeId;
            data.AddressType = new AddressTypeData().Map(address.AddressType);

            return data;
        }
    }
}