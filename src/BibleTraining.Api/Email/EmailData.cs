namespace BibleTraining.Api.Email
{
    using ContactType;
    using Improving.MediatR;

    public class EmailData : Resource<int>
    {
        public string          Address     { get; set; }
        public ContactTypeData ContactType { get; set; }
    }
}