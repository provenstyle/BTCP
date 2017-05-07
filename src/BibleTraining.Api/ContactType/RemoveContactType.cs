namespace BibleTraining.Api.ContactType
{
    using Improving.MediatR;

    public class RemoveContactType : UpdateResource<ContactTypeData, int>
    {
        public RemoveContactType()
        {
        }

        public RemoveContactType(ContactTypeData contactType)
            : base(contactType)
        {
        }
    }
}