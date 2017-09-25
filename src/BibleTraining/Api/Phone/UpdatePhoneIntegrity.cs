namespace BibleTraining.Api.Phone
{
    using FluentValidation;

    public class UpdatePhoneIntegrity : AbstractValidator<UpdatePhone>
    {
        public UpdatePhoneIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new PhoneDataIntegrity());
        }

        private class PhoneDataIntegrity : AbstractValidator<PhoneData>
        {
            public PhoneDataIntegrity()
            {
                RuleFor(x => x.Id)
                    .NotNull();
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}
