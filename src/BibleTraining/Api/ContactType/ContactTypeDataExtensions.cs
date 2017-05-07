namespace BibleTraining.Api.ContactType
{
    using Entities;

    public static class ContactTypeDataExtensions
    {
        public static ContactTypeData Map(this ContactTypeData data, EmailType emailType)
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