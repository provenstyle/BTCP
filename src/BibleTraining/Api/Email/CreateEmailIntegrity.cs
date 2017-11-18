namespace BibleTraining.Api.Email
{
    using Entities;
    using FluentValidation;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Mediate;

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
                //RuleFor(x => x.PersonId)
                    //.NotNull();
                    //.When(NoPersonIsStashed);
                RuleFor(x => x.EmailTypeId)
                    .NotNull();
                RuleFor(x => x.Address)
                    .NotEmpty()
                    .EmailAddress();
            }

            private bool NoPersonIsStashed(EmailData emailData)
            {
                return HandleMethod.Composer.Proxy<IStash>()
                    .TryGet<Person>() == null;
            }
        }
    }
}