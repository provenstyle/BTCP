namespace BibleTraining.Api.Phone
{
    using Entities;
    using Miruken.Callback;
    using Miruken.Map;

    public class PhoneMaps : Handler
    {
        [Maps]
        public PhoneData MapPhone(Phone phone, Mapping mapping)
        {
            var target = mapping.Target as PhoneData ?? new PhoneData();

            ResourceMapper.Map(target, phone);

            target.Name        = phone.Name;

            return target;
        }

        [Maps]
        public Phone MapPhoneData(PhoneData data, Mapping mapping)
        {
            var target = mapping.Target as Phone ?? new Phone();

            EntityMapper.Map(target, data);

            if (data.Name != null)
                target.Name = data.Name;

            return target;
        }
    }

}
