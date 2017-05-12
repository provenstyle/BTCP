namespace BibleTraining.Api.Phone
{
    using Improving.MediatR;

    public class GetPhones : Request.WithResponse<PhoneResult>
    {
        public GetPhones()
        {
            Ids = new int[0];
        }

        public GetPhones(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}