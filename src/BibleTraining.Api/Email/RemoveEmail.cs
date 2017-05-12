namespace BibleTraining.Api.Email
{
    using Improving.MediatR;

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