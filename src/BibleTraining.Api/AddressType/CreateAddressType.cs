namespace BibleTraining.Api.AddressType
{
    using Improving.MediatR;

    public class CreateAddressType : ResourceAction<AddressTypeData, int?>
    {
        public CreateAddressType()
        {
        }

        public CreateAddressType(AddressTypeData addressType)
            : base (addressType)
        {
        }
    }
}