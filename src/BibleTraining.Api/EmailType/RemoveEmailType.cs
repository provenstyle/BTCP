namespace BibleTraining.Api.EmailType
{
    using Improving.MediatR;

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