namespace BibleTraining.Api.EmailType
{
    using Entities;

    public static class EmailTypeExtensions
    {
        public static EmailType Map(this EmailType emailType, EmailTypeData data)
        {
            if (data == null) return null;

            EntityMapper.Map(emailType, data);

            if (data.Name != null)
                emailType.Name = data.Name;

            return emailType;
        }
    }
}