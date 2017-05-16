namespace BibleTraining.Api.PhoneType
{
    using Improving.MediatR;

    public class CreatePhoneType : ResourceAction<PhoneTypeData, int?>
    {
        public CreatePhoneType()
        {
        }

        public CreatePhoneType(PhoneTypeData phoneType)
            : base (phoneType)
        {
        }
    }
}
