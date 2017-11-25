namespace BibleTraining.Api.Email
{
    using EmailType;
    using Entities;
    using Miruken.Callback;
    using Miruken.Map;

    public class EmailMaps : Handler
    {
        [Maps]
        public Email Map(EmailData data, Mapping mapping)
        {
            var target = mapping.Target as Email ?? new Email();

            EntityMapper.Map(target, data);

            if (data.PersonId.HasValue)
                target.PersonId = data.PersonId.Value;

            if (data.EmailTypeId.HasValue)
                target.EmailTypeId = data.EmailTypeId.Value;

            if (data.Address != null)
                target.Address = data.Address;

            return target;
        }

        [Maps]
        public EmailData Map(
            Email email, Mapping mapping, [Proxy]IMapping mapper)
        {
            var target = mapping.Target as EmailData ?? new EmailData();
            ResourceMapper.Map(target, email);

            target.Address    = email.Address;
            target.PersonId   = email.PersonId;

            target.EmailTypeId = email.EmailTypeId;
            if(email.EmailType != null)
                target.EmailType = mapper.Map<EmailTypeData>(email.EmailType);

            return target;
        }
    }
}
