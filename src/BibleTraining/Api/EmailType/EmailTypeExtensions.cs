namespace BibleTraining.Api.EmailType
{
    using System;
    using Entities;

    public static class EmailTypeExtensions
    {
        public static EmailType Map(this EmailType emailType, EmailTypeData data)
        {
            if(data.Id.HasValue)
                emailType.Id = data.Id.Value;

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