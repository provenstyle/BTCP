namespace BibleTraining.Api.Email
{
    using Improving.MediatR;

    public class GetEmails : Request.WithResponse<EmailResult>
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