namespace BibleTraining.Api.Phone
{
    using Api;
    using Entities;

    public static class PhoneDataExtensions
    {
        public static PhoneData Map(this PhoneData data, Phone phone)
        {
            ResourceMapper.Map(data, phone);

            data.Name        = phone.Name;
            data.Description = phone.Description;

            data.PersonId    = phone.PersonId;

            return data;
        }
    }
}