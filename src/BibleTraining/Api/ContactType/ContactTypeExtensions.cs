namespace BibleTraining.Api.ContactType
{
    using System;
    using Entities;

    public static class ContactTypeExtensions
    {
        public static EmailType Map(this EmailType emailType, ContactTypeData data)
        {
            if (data.Name != null)
                emailType.Name = data.Name;

            if (data.CreatedBy != null)
                emailType.CreatedBy = data.CreatedBy;

            if (data.ModifiedBy != null)
                emailType.ModifiedBy = data.ModifiedBy;

            emailType.Modified = DateTime.Now;

            return emailType;
        }
    }
}