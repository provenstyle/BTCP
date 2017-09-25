namespace BibleTraining.Api.AddressType
{
    using Entities;
    using Miruken.Callback;
    using Miruken.Map;

    public class AddressTypeMaps : Handler
    {
        [Maps]
        public AddressTypeData MapAddressType(AddressType addressType, Mapping mapping)
        {
            var target = mapping.Target as AddressTypeData ?? new AddressTypeData();

            ResourceMapper.Map(target, addressType);

            target.Name        = addressType.Name;

            return target;
        }

        [Maps]
        public AddressType MapAddressTypeData(AddressTypeData data, Mapping mapping)
        {
            var target = mapping.Target as AddressType ?? new AddressType();

            EntityMapper.Map(target, data);

            if (data.Name != null)
                target.Name = data.Name;

            return target;
        }
    }

}
