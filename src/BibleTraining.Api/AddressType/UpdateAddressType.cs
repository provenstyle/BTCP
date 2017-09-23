namespace BibleTraining.Api.AddressType
{
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