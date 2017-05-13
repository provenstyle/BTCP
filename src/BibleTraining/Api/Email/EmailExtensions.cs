namespace BibleTraining.Api.Email
{
    using Entities;

    public static class EmailExtensions
    {
        public static Email Map(this Email email, EmailData data)
        {
            if (data == null) return null;

            EntityMapper.Map(email, data);

            if (data.PersonId.HasValue)
                email.PersonId = data.PersonId.Value;

            if (data.EmailTypeId.HasValue)
                email.EmailTypeId = data.EmailTypeId.Value;

            if (data.Address != null)
                email.Address = data.Address;

            return email;
        }
    }
}