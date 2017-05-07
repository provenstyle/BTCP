namespace BibleTraining.Api.ContactType
{
    using FluentValidation;

    public class CreateContactTypeIntegrity : AbstractValidator<CreateContactType>
    {
        public CreateContactTypeIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new ContactTypeDataIntegrity());
        }

        private class ContactTypeDataIntegrity : AbstractValidator<ContactTypeData>
        {
            public ContactTypeDataIntegrity()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}