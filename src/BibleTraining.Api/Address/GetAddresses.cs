namespace BibleTraining.Api.Address
{
    using Improving.MediatR;

    public class GetAddresses : Request.WithResponse<AddressResult>
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