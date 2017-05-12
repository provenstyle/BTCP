namespace BibleTraining.Api.Phone
{
    using Improving.MediatR;

    public class RemovePhone : UpdateResource<PhoneData, int?>
    {
        public RemovePhone()
        {
        }

        public RemovePhone(PhoneData phone)
            : base(phone)
        {
        }
    }
}