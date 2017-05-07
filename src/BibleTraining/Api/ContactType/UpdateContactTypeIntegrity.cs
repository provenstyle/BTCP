namespace BibleTraining.Api.ContactType
{
    using FluentValidation;

    public class UpdateContactTypeIntegrity : AbstractValidator<UpdateContactType>
    {
        public UpdateContactTypeIntegrity()
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