namespace BibleTraining.Api.Email
{
    using System;
    using ContactType;
    using Entities;

    public static class EmailExtensions
    {
        public static Email Map(this Email email, EmailData data)
        {
            if (data.Address != null)
                email.Address = data.Address;

            if (data.ContactType != null)
                email.EmailType.Map(data.ContactType);

            if (data.CreatedBy != null)
                email.CreatedBy = data.CreatedBy;

            if (data.ModifiedBy != null)
                email.ModifiedBy = data.ModifiedBy;

            email.Modified = DateTime.Now;

            return email;
        }
    }
}