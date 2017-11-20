namespace BibleTraining.Api.AddressType
{
    using FluentValidation;

    public class CreateUpdateAddressTypeIntegrity : AbstractValidator<IValidateCreateUpdateAddressType>
    {
        public CreateUpdateAddressTypeIntegrity()
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
