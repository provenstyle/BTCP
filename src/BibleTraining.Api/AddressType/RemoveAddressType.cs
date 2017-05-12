namespace BibleTraining.Api.AddressType
{
    using Improving.MediatR;

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