namespace BibleTraining.Api.Phone
{
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