namespace BibleTraining.Test._CodeGeneration
{
    using Api.Address;
    using Improving.MediatR;

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