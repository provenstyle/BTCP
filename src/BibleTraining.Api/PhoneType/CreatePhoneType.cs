namespace BibleTraining.Api.PhoneType
{
    public class CreatePhoneType : ResourceAction<PhoneTypeData, int?>
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
