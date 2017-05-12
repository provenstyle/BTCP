namespace BibleTraining.Api.Email
{
    using System;
    using Entities;

    public static class EmailExtensions
    {
        public static Email Map(this Email email, EmailData data)
        {
            if (data.Id.HasValue)
                email.Id = data.Id.Value;

            if (data.PersonId.HasValue)
                email.PersonId = data.PersonId.Value;

            if (data.Address != null)
                email.Address = data.Address;

            if (data.EmailTypeId.HasValue)
                email.EmailTypeId = data.EmailTypeId.Value;

            if (data.CreatedBy != null)
                email.CreatedBy = data.CreatedBy;

            if (data.ModifiedBy != null)
                email.ModifiedBy = data.ModifiedBy;

            email.Modified = DateTime.Now;

            return email;
        }
    }
}