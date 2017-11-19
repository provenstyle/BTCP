using Miruken.Validate.FluentValidation;

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
                RuleFor(x => x.PersonId)
                    .WithComposer(HasPersonOrId)
                    .WithoutComposer(HasPersonId);
                RuleFor(x => x.EmailTypeId)
                    .NotNull();
                RuleFor(x => x.Address)
                    .NotEmpty()
                    .EmailAddress();
            }

            private static bool HasPersonOrId(
                EmailData emailData, int? personId, IHandler composer)
            {
                return composer.Proxy<IStash>()
                    .TryGet<Person>() != null || HasPersonId(emailData, personId);
            }

            private static bool HasPersonId(EmailData emailData, int? personId)
            {
                return personId.HasValue;
            }
        }
    }
}