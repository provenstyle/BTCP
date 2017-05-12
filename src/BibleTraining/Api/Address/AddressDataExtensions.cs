namespace BibleTraining.Api.Address
{
    using Api;
    using Entities;

    public static class AddressDataExtensions
    {
        public static AddressData Map(this AddressData data, Address address)
        {
            ResourceMapper.Map(data, address);

            data.Name          = address.Name;
            data.Description   = address.Description;

            data.PersonId      = address.PersonId;
            data.AddressTypeId = address.AddressTypeId;

            return data;
        }
    }
}