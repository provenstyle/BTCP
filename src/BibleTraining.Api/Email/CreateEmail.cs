namespace BibleTraining.Api.Email
{
    public class CreateEmail :
        ResourceAction<EmailData, int?>,
        IValidateCreateUpdateEmail
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