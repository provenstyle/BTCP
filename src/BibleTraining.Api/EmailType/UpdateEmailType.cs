namespace BibleTraining.Api.EmailType
{
    public class UpdateEmailType : UpdateResource<EmailTypeData, int?>
    {
        public UpdateEmailType()
        {
        }

        public UpdateEmailType(EmailTypeData emailType)
            : base(emailType)
        {          
        }
    }
}
