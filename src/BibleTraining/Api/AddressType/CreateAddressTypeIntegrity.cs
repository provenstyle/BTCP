namespace BibleTraining.Api.AddressType
{
    using FluentValidation;

    public class CreateAddressTypeIntegrity : AbstractValidator<CreateAddressType>
    {
        public CreateAddressTypeIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new AddressTypeDataIntegrity());
        }

        private class AddressTypeDataIntegrity : AbstractValidator<AddressTypeData>
        {
            public AddressTypeDataIntegrity()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}