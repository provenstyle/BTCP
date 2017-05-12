namespace BibleTraining.Api.Phone
{
    using Improving.MediatR;

    public class CreatePhone : ResourceAction<PhoneData, int?>
    {
        public CreatePhone()
        {
        }

        public CreatePhone(PhoneData phone)
            : base (phone)
        {
        }
    }
}