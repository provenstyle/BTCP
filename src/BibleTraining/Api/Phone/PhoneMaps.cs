namespace BibleTraining.Api.Phone
{
    using Entities;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Map;
    using PhoneType;

    public class PhoneMaps : Handler
    {
        [Maps]
        public PhoneData MapPhone(Phone phone, Mapping mapping, IHandler composer)
        {
            var target = mapping.Target as PhoneData ?? new PhoneData();

            ResourceMapper.Map(target, phone);

            target.Name      = phone.Name;

            if(phone.PhoneType != null)
                target.PhoneType = composer.Proxy<IMapping>().Map<PhoneTypeData>(phone.PhoneType);

            return target;
        }

        [Maps]
        public Phone MapPhoneData(PhoneData data, Mapping mapping, IHandler composer)
        {
            var target = mapping.Target as Phone ?? new Phone();

            EntityMapper.Map(target, data);

            if (data.Name != null)
                target.Name = data.Name;

            //if (data.PhoneType != null)
            //    target.PhoneType = composer.Proxy<IMapping>().Map<PhoneType>(data.PhoneType);

            return target;
        }
    }

}
