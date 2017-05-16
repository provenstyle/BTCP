namespace BibleTraining.Api.PhoneType
{
    using Improving.MediatR;

    public class GetPhoneTypes : Request.WithResponse<PhoneTypeResult>
    {
        public GetPhoneTypes()
        {
            Ids = new int[0];
        }

        public GetPhoneTypes(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}
