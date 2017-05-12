namespace BibleTraining.Api.Address
{
    using Improving.MediatR;

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