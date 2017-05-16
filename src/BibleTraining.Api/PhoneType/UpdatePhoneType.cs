namespace BibleTraining.Api.PhoneType
{
    using Improving.MediatR;

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
