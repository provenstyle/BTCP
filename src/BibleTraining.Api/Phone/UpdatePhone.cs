namespace BibleTraining.Api.Phone
{
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