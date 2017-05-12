namespace BibleTraining.Api.Phone
{
    using Api;
    using Entities;

    public static class PhoneExtensions
    {
        public static Phone Map(this Phone phone, PhoneData data)
        {
            EntityMapper.Map(phone, data);

            if (data.Name != null)
                phone.Name = data.Name;

            if (data.Description != null)
                phone.Description = data.Description;

            if (data.PersonId.HasValue)
                phone.PersonId = data.PersonId.Value;

            return phone;
        }
    }
}