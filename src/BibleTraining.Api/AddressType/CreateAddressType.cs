namespace BibleTraining.Api.AddressType
{
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