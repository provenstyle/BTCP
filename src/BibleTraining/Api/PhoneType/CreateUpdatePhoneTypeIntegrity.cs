namespace BibleTraining.Api.PhoneType
{
    using FluentValidation;

    public class CreateUpdatePhoneTypeIntegrity : AbstractValidator<IValidateCreateUpdatePhoneType>
    {
        public CreateUpdatePhoneTypeIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new PhoneTypeDataIntegrity());
        }

        private class PhoneTypeDataIntegrity : AbstractValidator<PhoneTypeData>
        {
            public PhoneTypeDataIntegrity()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}
