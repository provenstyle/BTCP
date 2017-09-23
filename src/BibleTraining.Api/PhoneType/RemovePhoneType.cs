namespace BibleTraining.Api.PhoneType
{
    public class RemovePhoneType : UpdateResource<PhoneTypeData, int?>
    {
        public RemovePhoneType()
        {
        }

        public RemovePhoneType(PhoneTypeData phoneType)
            : base(phoneType)
        {
        }
    }
}
