namespace BibleTraining.Api.PhoneType
{
    using Miruken.Mediate;

    public class GetPhoneTypes : IRequest<PhoneTypeResult>
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
