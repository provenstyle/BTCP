namespace BibleTraining.Api.PhoneType 
{
    using Improving.MediatR;

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
