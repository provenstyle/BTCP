namespace BibleTraining.Api.EmailType
{
    using Entities;

    public static class EmailTypeDataExtensions
    {
        public static EmailTypeData Map(this EmailTypeData data, EmailType emailType)
        {
            data.Id = emailType.Id;
            data.Name = emailType.Name;
            data.RowVersion = emailType.RowVersion;
            data.CreatedBy = emailType.CreatedBy;
            data.Created = emailType.Created;
            data.ModifiedBy = emailType.ModifiedBy;
            data.Modified = emailType.Modified;

            return data;
        }
    }
}