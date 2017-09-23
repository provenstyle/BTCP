namespace BibleTraining.Api.Address
{
    public class RemoveAddress : UpdateResource<AddressData, int?>
    {
        public RemoveAddress()
        {
        }

        public RemoveAddress(AddressData address)
            : base(address)
        {
        }
    }
}