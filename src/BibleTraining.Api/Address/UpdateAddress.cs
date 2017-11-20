namespace BibleTraining.Api.Address
{
    using Api;

    public class UpdateAddress :
        UpdateResource<AddressData, int?>,
        IValidateAddressCreateUpdate
    {
        public UpdateAddress()
        {
        }

        public UpdateAddress(AddressData address)
            : base(address)
        {
        }
    }
}