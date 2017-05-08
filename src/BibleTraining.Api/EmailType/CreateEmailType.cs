namespace BibleTraining.Api.EmailType
{
    using Improving.MediatR;

    public class CreateEmailType : ResourceAction<EmailTypeData, int>
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