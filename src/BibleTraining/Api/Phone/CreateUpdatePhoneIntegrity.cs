namespace BibleTraining.Api.Phone
{
    using Entities;
    using FluentValidation;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Mediate;
    using Miruken.Validate.FluentValidation;

    public class CreateUpdatePhoneIntegrity : AbstractValidator<IValidateCreateUpdatePhone>
    {
        public CreateUpdatePhoneIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new PhoneDataIntegrity());
        }

        private class PhoneDataIntegrity : AbstractValidator<PhoneData>
        {
            public PhoneDataIntegrity()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
                RuleFor(x => x.PhoneTypeId)
                    .NotNull();
                RuleFor(x => x.PersonId)
                    .WithComposer(HasPersonOrId)
                    .WithoutComposer(HasPersonId);
            }
        }

        private static bool HasPersonOrId(
            PhoneData phoneData, int? personId, IHandler composer)
        {
            return composer.Proxy<IStash>().TryGet<Person>() != null
                || HasPersonId(phoneData, personId);
        }

        private static bool HasPersonId(PhoneData phoneData, int? personId)
        {
            return personId.HasValue;
        }
    }
}
