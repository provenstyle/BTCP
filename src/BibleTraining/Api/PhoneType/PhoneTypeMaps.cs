namespace BibleTraining.Api.PhoneType
{
    using Entities;
    using Miruken.Callback;
    using Miruken.Map;

    public class PhoneTypeMaps : Handler
    {
        [Maps]
        public PhoneTypeData MapPhoneType(PhoneType phoneType, Mapping mapping)
        {
            var target = mapping.Target as PhoneTypeData ?? new PhoneTypeData();

            ResourceMapper.Map(target, phoneType);

            target.Name        = phoneType.Name;

            return target;
        }

        [Maps]
        public PhoneType MapPhoneTypeData(PhoneTypeData data, Mapping mapping)
        {
            var target = mapping.Target as PhoneType ?? new PhoneType();

            EntityMapper.Map(target, data);

            if (data.Name != null)
                target.Name = data.Name;

            return target;
        }
    }

}
