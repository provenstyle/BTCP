namespace BibleTraining.Api.EmailType
{
    using Entities;
    using Miruken.Callback;
    using Miruken.Map;

    public class EmailTypeMaps : Handler
    {
        [Maps]
        public EmailTypeData MapEmailType(EmailType emailType, Mapping mapping)
        {
            var target = mapping.Target as EmailTypeData ?? new EmailTypeData();

            ResourceMapper.Map(target, emailType);

            target.Name        = emailType.Name;

            return target;
        }

        [Maps]
        public EmailType MapEmailTypeData(EmailTypeData data, Mapping mapping)
        {
            var target = mapping.Target as EmailType ?? new EmailType();

            EntityMapper.Map(target, data);

            if (data.Name != null)
                target.Name = data.Name;

            return target;
        }
    }

}
