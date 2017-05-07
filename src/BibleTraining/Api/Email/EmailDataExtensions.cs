namespace BibleTraining.Api.Email
{
    using Entities;

    public static class EmailDataExtensions
    {
        public static EmailData Map(this EmailData data, Email email)
        {
            data.Id         = email.Id;
            data.Address    = email.Address;
            data.RowVersion = email.RowVersion;
            data.CreatedBy  = email.CreatedBy;
            data.Created    = email.Created;
            data.ModifiedBy = email.ModifiedBy;
            data.Modified   = email.Modified;

            return data;
        }
    }
}