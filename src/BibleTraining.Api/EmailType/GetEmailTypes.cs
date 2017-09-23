namespace BibleTraining.Api.EmailType
{
    using Miruken.Mediate;

    public class GetEmailTypes : IRequest<EmailTypeResult>
    {
        public GetEmailTypes()
        {
            Ids = new int[0];
        }

        public GetEmailTypes(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set; }

        public bool KeyProperties { get; set; }
    }
}