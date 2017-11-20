namespace BibleTraining.Api.Address
{
    public class CreateAddress :
        ResourceAction<AddressData, int?>,
        IValidateAddressCreateUpdate
    {
        public CreateAddress()
        {
        }

        public CreateAddress(AddressData address)
            : base (address)
        {
        }
    }
}