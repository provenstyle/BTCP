namespace BibleTraining.Api.Email
{
    using Miruken.Mediate;

    public class GetEmails : IRequest<EmailResult>
    {
        public GetEmails()
        {
            Ids = new int[0];
        }

        public GetEmails(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}