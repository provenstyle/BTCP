namespace BibleTraining.Api.Phone
{
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
