namespace BibleTraining.Api.Email
{
    public class UpdateEmail :
        UpdateResource<EmailData, int?>,
        IValidateCreateUpdateEmail
    {
        public UpdateEmail()
        {
        }

        public UpdateEmail(EmailData email)
            : base(email)
        {
        }
    }
}