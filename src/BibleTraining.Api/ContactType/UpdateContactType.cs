namespace BibleTraining.Api.ContactType
{
    using Improving.MediatR;

    public class UpdateContactType : UpdateResource<ContactTypeData, int>
    {
        public UpdateContactType()
        {
        }

        public UpdateContactType(ContactTypeData contactType)
            : base(contactType)
        {
        }
    }
}