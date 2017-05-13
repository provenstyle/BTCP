namespace BibleTraining.Api.Phone
{
    using Api;
    using Entities;
    using PhoneType;

    public static class PhoneDataExtensions
    {
        public static PhoneData Map(this PhoneData data, Phone phone)
        {
            if (phone == null) return null;

            ResourceMapper.Map(data, phone);

            data.Name        = phone.Name;
            data.Description = phone.Description;

            data.PersonId    = phone.PersonId;

            data.PhoneTypeId = phone.PhoneTypeId;
            data.PhoneType   = new PhoneTypeData().Map(phone.PhoneType);

            return data;
        }
    }
}