namespace BibleTraining.Api.EmailType
{
    using FluentValidation;

    public class CreateEmailTypeIntegrity : AbstractValidator<CreateEmailType>
    {
        public CreateEmailTypeIntegrity()
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