namespace BibleTraining.Api.AddressType
{
    public class CreateAddressType :
        ResourceAction<AddressTypeData, int?>,
        IValidateCreateUpdateAddressType
    {
        public CreateAddressType()
        {
        }

        public CreateAddressType(AddressTypeData addressType)
            : base(addressType)
        {
        }
    }
}
