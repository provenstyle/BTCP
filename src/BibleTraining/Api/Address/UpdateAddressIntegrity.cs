namespace BibleTraining.Api.Address
{
    using FluentValidation;
    using Test._CodeGeneration;

    public class UpdateAddressIntegrity : AbstractValidator<UpdateAddress>
    {
        public UpdateAddressIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new AddressDataIntegrity());
        }

        private class AddressDataIntegrity : AbstractValidator<AddressData>
        {
            public AddressDataIntegrity()
            {
                RuleFor(x => x.Id)
                    .NotNull();
                RuleFor(x => x.PersonId)
                    .NotNull();
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}