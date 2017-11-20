namespace BibleTraining.Api.PhoneType
{
    public class UpdatePhoneType :
        UpdateResource<PhoneTypeData, int?>,
        IValidateCreateUpdatePhoneType
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
