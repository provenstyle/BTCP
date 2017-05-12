namespace BibleTraining.Api.AddressType
{
    using Improving.MediatR;

    public class UpdateAddressType : UpdateResource<AddressTypeData, int?>
    {
        public UpdateAddressType()
        {
        }

        public UpdateAddressType(AddressTypeData addressType)
            : base(addressType)
        {
        }
    }
}