namespace BibleTraining.Api.Email
{
    using FluentValidation;

    public class CreateEmailIntegrity : AbstractValidator<CreateEmail>
    {
        public CreateEmailIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new EmailDataIntegrity());
        }

        private class EmailDataIntegrity : AbstractValidator<EmailData>
        {
            public EmailDataIntegrity()
            {
                RuleFor(x => x.PersonId)
                    .NotNull();
                RuleFor(x => x.EmailTypeId)
                    .NotNull();
                RuleFor(x => x.Address)
                    .NotEmpty();
            }
        }
    }
}