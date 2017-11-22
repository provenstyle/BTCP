namespace BibleTraining.Api.PhoneType
{
    public class CreatePhoneType : 
        ResourceAction<PhoneTypeData, int?>,
        IValidateCreateUpdatePhoneType
    {
        public CreatePhoneType()
        {
        }

        public CreatePhoneType(PhoneTypeData phoneType)
            : base(phoneType)
        {
        }
    }
}
