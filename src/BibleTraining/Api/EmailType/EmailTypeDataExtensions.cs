namespace BibleTraining.Api.EmailType
{
    using Entities;

    public static class EmailTypeDataExtensions
    {
        public static EmailTypeData Map(this EmailTypeData data, EmailType emailType)
        {
            if (emailType == null) return null;

            ResourceMapper.Map(data, emailType);

            data.Name = emailType.Name;

            return data;
        }
    }
}