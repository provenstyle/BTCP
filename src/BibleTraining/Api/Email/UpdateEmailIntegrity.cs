namespace BibleTraining.Api.Email
{
    using FluentValidation;

    public class UpdateEmailIntegrity : AbstractValidator<UpdateEmail>
    {
        public UpdateEmailIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new EmailDataIntegrity());
        }

        private class EmailDataIntegrity : AbstractValidator<EmailData>
        {
            public EmailDataIntegrity()
            {
                RuleFor(x => x.Id)
                    .NotNull();
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