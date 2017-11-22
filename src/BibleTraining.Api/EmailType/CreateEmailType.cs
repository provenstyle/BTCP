namespace BibleTraining.Api.EmailType
{
    public class CreateEmailType : 
        ResourceAction<EmailTypeData, int?>,
        IValidateCreateUpdateEmailType
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
