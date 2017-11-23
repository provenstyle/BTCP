namespace BibleTraining.Api.Phone
{
    using Miruken.Mediate;

    public class GetPhones : IRequest<PhoneResult>
    {
        public GetPhones()
        {
            Ids = new int[0];
        }

        public GetPhones(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set; }
    }
}
