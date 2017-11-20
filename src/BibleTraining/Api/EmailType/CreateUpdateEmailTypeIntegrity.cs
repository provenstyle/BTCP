namespace BibleTraining.Api.EmailType
{
    using FluentValidation;

    public class CreateUpdateEmailTypeIntegrity : AbstractValidator<IValidateCreateUpdateEmailType>
    {
        public CreateUpdateEmailTypeIntegrity()
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
