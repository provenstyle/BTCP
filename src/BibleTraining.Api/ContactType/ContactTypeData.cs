namespace BibleTraining.Api.ContactType
{
    using Improving.MediatR;

    public class ContactTypeData : Resource<int>
    {
        public string Name { get; set; }

    }
}