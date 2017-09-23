namespace BibleTraining.Api.Email
{
    public class UpdateEmail : UpdateResource<EmailData, int?>
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