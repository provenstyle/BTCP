namespace BibleTraining.Api.EmailType
{
    using FluentValidation;

    public class UpdateEmailTypeIntegrity : AbstractValidator<UpdateEmailType>
    {
        public UpdateEmailTypeIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new EmailTypeDataIntegrity());
        }

        private class EmailTypeDataIntegrity : AbstractValidator<EmailTypeData>
        {
            public EmailTypeDataIntegrity()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}