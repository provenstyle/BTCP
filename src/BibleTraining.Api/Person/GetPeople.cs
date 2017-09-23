namespace BibleTraining.Api.Person
{
    using Miruken.Mediate;

    public class GetPeople : IRequest<PersonResult>
    {
        public bool IncludeEmails    { get; set; }
        public bool IncludeAddresses { get; set; }
        public bool IncludePhones    { get; set; }

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