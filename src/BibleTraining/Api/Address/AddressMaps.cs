namespace BibleTraining.Api.Address
{
    using AddressType;
    using Entities;
    using Miruken.Callback;
    using Miruken.Map;

    public class AddressMaps : Handler
    {
        [Maps]
        public Address MapAddressData(AddressData data, Mapping mapping)
        {
            var target = mapping.Target as Address ?? new Address();

            EntityMapper.Map(target, data);

            if (data.Name != null)
                target.Name = data.Name;

            if (data.Description != null)
                target.Description = data.Description;

            if (data.PersonId.HasValue)
                target.PersonId = data.PersonId.Value;

            if (data.AddressTypeId.HasValue)
                target.AddressTypeId = data.AddressTypeId.Value;

            return target;
        }

        [Maps]
        public AddressData Map(
            Address address, Mapping mapping, [Proxy]IMapping mapper)
        {
            var target = mapping.Target as AddressData ?? new AddressData();

            ResourceMapper.Map(target, address);

            target.Name          = address.Name;
            target.Description   = address.Description;
            target.PersonId      = address.PersonId;
            target.AddressTypeId = address.AddressTypeId;

            if(address.AddressType != null)
                target.AddressType = mapper.Map<AddressTypeData>(address.AddressType);

            return target;
        }
    }
}
