namespace BibleTraining.Api.Email
{
    public class RemoveEmail : UpdateResource<EmailData, int?>
    {
        public RemoveEmail()
        {
        }

        public RemoveEmail(EmailData email)
            : base(email)
        {
        }
    }
}