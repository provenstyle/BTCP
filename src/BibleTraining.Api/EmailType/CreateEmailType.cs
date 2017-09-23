namespace BibleTraining.Api.EmailType
{
    public class CreateEmailType : ResourceAction<EmailTypeData, int?>
    {
        public CreateEmailType()
        {
        }

        public CreateEmailType(EmailTypeData emailType)
            : base(emailType)
        {
        }
    }
}