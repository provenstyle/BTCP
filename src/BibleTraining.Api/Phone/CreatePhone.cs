namespace BibleTraining.Api.Phone
{
    public class CreatePhone : 
        ResourceAction<PhoneData, int?>,
        IValidateCreateUpdatePhone
    {
        public CreatePhone()
        {
        }

        public CreatePhone(PhoneData phone)
            : base(phone)
        {
        }
    }
}
