namespace BibleTraining.Api.Address
{
    using Miruken.Mediate;

    public class GetAddresses : IRequest<AddressResult>
    {
        public GetAddresses()
        {
            Ids = new int[0];
        }

        public GetAddresses(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}