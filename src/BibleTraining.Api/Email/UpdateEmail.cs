namespace BibleTraining.Api.Email
{
    using Improving.MediatR;

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