namespace BibleTraining.Api.Phone
{
    using Api;
    using Entities;

    public static class PhoneExtensions
    {
        public static Phone Map(this Phone phone, PhoneData data)
        {
            if (data == null) return null;

            EntityMapper.Map(phone, data);

            if (data.Name != null)
                phone.Name = data.Name;

            if (data.Description != null)
                phone.Description = data.Description;

            if (data.PersonId.HasValue)
                phone.PersonId = data.PersonId.Value;

            if (data.PhoneTypeId.HasValue)
                phone.PhoneTypeId = data.PhoneTypeId.Value;

            return phone;
        }
    }
}