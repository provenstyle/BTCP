namespace BibleTraining.Api.Phone
{
    public class UpdatePhone :
        UpdateResource<PhoneData, int?>,
        IValidateCreateUpdatePhone
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
