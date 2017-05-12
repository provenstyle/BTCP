namespace BibleTraining.Api.Address
{
    using Improving.MediatR;

    public class CreateAddress : ResourceAction<AddressData, int?>
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