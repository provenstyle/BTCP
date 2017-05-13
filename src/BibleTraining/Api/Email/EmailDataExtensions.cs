namespace BibleTraining.Api.Email
{
    using EmailType;
    using Entities;

    public static class EmailDataExtensions
    {
        public static EmailData Map(this EmailData data, Email email)
        {
            ResourceMapper.Map(data, email);

            data.Address    = email.Address;
            data.PersonId   = email.PersonId;

            data.EmailTypeId = email.EmailTypeId;
            data.EmailType   = new EmailTypeData().Map(email.EmailType);

            return data;
        }
    }
}