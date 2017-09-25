namespace BibleTraining.Api.AddressType
{
    using FluentValidation;

    public class UpdateAddressTypeIntegrity : AbstractValidator<UpdateAddressType>
    {
        public UpdateAddressTypeIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new AddressTypeDataIntegrity());
        }

        private class AddressTypeDataIntegrity : AbstractValidator<AddressTypeData>
        {
            public AddressTypeDataIntegrity()
            {
                RuleFor(x => x.Id)
                    .NotNull();
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}
