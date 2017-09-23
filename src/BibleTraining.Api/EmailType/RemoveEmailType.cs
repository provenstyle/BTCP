namespace BibleTraining.Api.EmailType
{
    public class RemoveEmailType : UpdateResource<EmailTypeData, int?>
    {
        public RemoveEmailType()
        {
        }

        public RemoveEmailType(EmailTypeData emailType)
            : base(emailType)
        {
        }
    }
}