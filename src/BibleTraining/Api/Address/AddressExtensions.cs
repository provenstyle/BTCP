namespace BibleTraining.Api.Address
{
    using Api;
    using Entities;

    public static class AddressExtensions
    {
        public static Address Map(this Address address, AddressData data)
        {
            EntityMapper.Map(address, data);

            if (data.Name != null)
                address.Name = data.Name;

            if (data.Description != null)
                address.Description = data.Description;

            if (data.PersonId.HasValue)
                address.PersonId = data.PersonId.Value;

            return address;
        }
    }
}