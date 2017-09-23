namespace BibleTraining.Api.AddressType
{
    public class RemoveAddressType : UpdateResource<AddressTypeData, int?>
    {
        public RemoveAddressType()
        {
        }

        public RemoveAddressType(AddressTypeData addressType)
            : base(addressType)
        {
        }
    }
}