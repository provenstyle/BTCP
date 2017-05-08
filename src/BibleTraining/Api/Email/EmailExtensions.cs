namespace BibleTraining.Api.Email
{
    using System;
    using Entities;

    public static class EmailExtensions
    {
        public static Email Map(this Email email, EmailData data)
        {
            if (data.Address != null)
                email.Address = data.Address;

            if (data.EmailType?.Id > 0)
                email.EmailTypeId = data.EmailType.Id;

            if (data.CreatedBy != null)
                email.CreatedBy = data.CreatedBy;

            if (data.ModifiedBy != null)
                email.ModifiedBy = data.ModifiedBy;

            email.Modified = DateTime.Now;

            return email;
        }
    }
}