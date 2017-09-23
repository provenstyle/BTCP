namespace BibleTraining.Api.Address
{
    using Api;

    public class UpdateAddress : UpdateResource<AddressData, int?>
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