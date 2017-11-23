namespace BibleTraining.Api.Phone
{
    using Entities;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Map;
    using Person;
    using PhoneType;

    public class PhoneMaps : Handler
    {
        [Maps]
        public Phone MapPhoneData(PhoneData data, Mapping mapping, IHandler composer)
        {
            var target = mapping.Target as Phone ?? new Phone();

            EntityMapper.Map(target, data);

            if (data.Number != null)
                target.Number = data.Number;

            if (data.Extension != null)
                target.Extension = data.Extension;

            if (data.PhoneTypeId.HasValue)
                target.PhoneTypeId = data.PhoneTypeId.Value;

            if (data.PersonId.HasValue)
                target.PersonId = data.PersonId.Value;

            return target;
        }

        [Maps]
        public PhoneData MapPhone(Phone phone, Mapping mapping, IHandler composer)
        {
            var target = mapping.Target as PhoneData ?? new PhoneData();

            ResourceMapper.Map(target, phone);

            target.Number      = phone.Number;
            target.Extension   = phone.Extension;
            target.PhoneTypeId = phone.PhoneTypeId;
            target.PersonId    = phone.PersonId;

            if(phone.PhoneType != null)
                target.PhoneType = composer.Proxy<IMapping>().Map<PhoneTypeData>(phone.PhoneType);

            return target;
        }
    }
}
