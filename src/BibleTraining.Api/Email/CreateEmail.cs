namespace BibleTraining.Api.Email
{
    public class CreateEmail : ResourceAction<EmailData, int?>
    {
        public CreateEmail()
        {
        }

        public CreateEmail(EmailData email)
            : base (email)
        {
        }
    }
}