namespace BibleTraining.Api.ContactType
{
    using Improving.MediatR;

    public class CreateContactType : ResourceAction<ContactTypeData, int>
    {
        public CreateContactType()
        {
        }

        public CreateContactType(ContactTypeData contactType)
            : base(contactType)
        {
        }
    }
}