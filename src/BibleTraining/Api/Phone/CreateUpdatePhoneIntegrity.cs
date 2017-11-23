namespace BibleTraining.Api.Phone
{
    using Entities;
    using FluentValidation;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Mediate;
    using Miruken.Validate.FluentValidation;
    using PhoneNumbers;

    public class CreateUpdatePhoneIntegrity
        : AbstractValidator<IValidateCreateUpdatePhone>
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
                CascadeMode = CascadeMode.StopOnFirstFailure;

                RuleFor(x => x.Number)
                    .NotEmpty()
                    .Must(BeAValidPhoneNumber)
                    .WithMessage("Must be valid international phone number starting with country code.");
                RuleFor(x => x.PhoneTypeId)
                    .NotNull();
                RuleFor(x => x.PersonId)
                    .WithComposer(HasPersonOrId)
                    .WithoutComposer(HasPersonId);
            }

            private static bool BeAValidPhoneNumber(string number)
            {
                try
                {
                    var util = PhoneNumberUtil.GetInstance();
                    var parsed = util.Parse(number, RegionCode.US);
                    return util.IsValidNumber(parsed);
                }
                catch
                {
                    return false;
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
}
