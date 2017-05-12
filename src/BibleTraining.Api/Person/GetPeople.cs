namespace BibleTraining.Api.Person
{
    using Improving.MediatR;

    public class GetPeople : Request.WithResponse<PersonResult>
    {
        public bool IncludeEmails    { get; set; }
        public bool IncludeAddresses { get; set; }

        public GetPeople()
        {
            Ids = new int[0];
        }

        public GetPeople(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set;}

        public bool KeyProperties { get; set; }
    }
}