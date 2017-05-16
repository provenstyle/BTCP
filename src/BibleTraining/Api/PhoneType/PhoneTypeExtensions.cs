namespace BibleTraining.Api.PhoneType
{
    using Entities;

    public static class PhoneTypeExtensions
    {
        public static PhoneType Map(this PhoneType phoneType, PhoneTypeData data)
        {
            if (data == null) return null;

            EntityMapper.Map(phoneType, data);

            if (data.Name != null)
                phoneType.Name = data.Name;
                
            if (data.Description != null)
                phoneType.Description = data.Description;

            return phoneType;
        }
    }
}
