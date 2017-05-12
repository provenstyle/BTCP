namespace BibleTraining.Api.Phone
{
    using FluentValidation;

    public class CreatePhoneIntegrity : AbstractValidator<CreatePhone>
    {
        public CreatePhoneIntegrity()
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
            }
        }
    }
}