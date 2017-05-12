namespace BibleTraining.Api.Email
{
    using Improving.MediatR;

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