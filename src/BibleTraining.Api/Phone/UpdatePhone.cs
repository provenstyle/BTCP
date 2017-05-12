namespace BibleTraining.Api.Phone
{
    using Improving.MediatR;

    public class UpdatePhone : UpdateResource<PhoneData, int?>
    {
        public UpdatePhone()
        {
        }

        public UpdatePhone(PhoneData phone)
            : base(phone)
        {
        }
    }
}