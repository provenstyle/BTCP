namespace BibleTraining.Api.PhoneType
{
    using Entities;
    
    public static class PhoneTypeDataExtensions
    {
        public static PhoneTypeData Map(this PhoneTypeData data, PhoneType phoneType)
        {
            if (phoneType == null) return null;
            
            ResourceMapper.Map(data, phoneType);

            data.Name        = phoneType.Name;
            data.Description = phoneType.Description;

            return data;
        }
    }
}
