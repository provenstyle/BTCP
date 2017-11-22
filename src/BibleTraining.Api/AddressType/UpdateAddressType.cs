namespace BibleTraining.Api.AddressType
{
    public class UpdateAddressType :
        UpdateResource<AddressTypeData, int?>,
        IValidateCreateUpdateAddressType
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
