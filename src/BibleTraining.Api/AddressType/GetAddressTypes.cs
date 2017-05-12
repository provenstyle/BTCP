namespace BibleTraining.Api.AddressType
{
    using Improving.MediatR;

    public class GetAddressTypes : Request.WithResponse<AddressTypeResult>
    {
        public GetAddressTypes()
        {
            Ids = new int[0];
        }

        public GetAddressTypes(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}