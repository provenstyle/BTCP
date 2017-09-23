namespace BibleTraining.Api.PhoneType
{
    public class UpdatePhoneType : UpdateResource<PhoneTypeData, int?>
    {
        public UpdatePhoneType()
        {
        }

        public UpdatePhoneType(PhoneTypeData phoneType)
            : base(phoneType)
        {          
        }
    }
}
