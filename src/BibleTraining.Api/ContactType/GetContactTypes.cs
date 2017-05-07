namespace BibleTraining.Api.ContactType
{
    using Improving.MediatR;

    public class GetContactTypes : Request.WithResponse<ContactTypeResult>
    {
        public GetContactTypes()
        {
            Ids = new int[0];
        }

        public GetContactTypes(params int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set; }

        public bool KeyProperties { get; set; }
    }
}