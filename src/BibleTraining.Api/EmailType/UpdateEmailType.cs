namespace BibleTraining.Api.EmailType
{
    using Improving.MediatR;

    public class UpdateEmailType : UpdateResource<EmailTypeData, int>
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