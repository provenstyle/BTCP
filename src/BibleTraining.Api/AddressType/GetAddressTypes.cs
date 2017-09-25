namespace BibleTraining.Api.AddressType
{
    using Miruken.Mediate; 
    
    public class GetAddressTypes : IRequest<AddressTypeResult>
    {
        public GetAddressTypes()
        {
            Ids = new int[0];
        }

        public GetAddressTypes(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set; }

        public bool KeyProperties { get; set; }
    }
}
