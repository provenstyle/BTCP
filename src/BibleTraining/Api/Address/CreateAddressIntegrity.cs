namespace BibleTraining.Api.Address
{
    using FluentValidation;

    public class CreateAddressIntegrity : AbstractValidator<CreateAddress>
    {
        public CreateAddressIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new AddressDataIntegrity());
        }

        private class AddressDataIntegrity : AbstractValidator<AddressData>
        {
            public AddressDataIntegrity()
            {
                //RuleFor(x => x.PersonId)
                //    .NotNull();
                //RuleFor(x => x.AddressTypeId)
                //    .NotNull();
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}