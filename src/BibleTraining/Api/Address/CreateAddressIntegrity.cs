namespace BibleTraining.Api.Address
{
    using System;
    using Entities;
    using FluentValidation;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Mediate;
    using Miruken.Validate.FluentValidation;

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
                RuleFor(x => x.AddressTypeId)
                    .NotNull();
                RuleFor(x => x.Name)
                    .NotEmpty();
                RuleFor(x => x.PersonId)
                    .WithComposer(HasPersonIdOrPerson)
                    .WithoutComposer(HasPersonId);
            }

            private bool HasPersonId(AddressData addressData, int? personId)
            {
                return personId.HasValue;
            }

            private bool HasPersonIdOrPerson(AddressData addressData, int? i, IHandler composer)
            {
                return composer.Proxy<IStash>().TryGet<Person>() != null
                    || HasPersonId(addressData, i);
            }
        }
    }
}