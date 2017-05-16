namespace BibleTraining.Api.PhoneType
{
    using FluentValidation;

    public class UpdatePhoneTypeIntegrity : AbstractValidator<UpdatePhoneType>
    {
        public UpdatePhoneTypeIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new PhoneTypeDataIntegrity());
        }

        private class PhoneTypeDataIntegrity : AbstractValidator<PhoneTypeData>
        {
            public PhoneTypeDataIntegrity()
            {
                RuleFor(x => x.Id)
                    .NotNull();
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}
