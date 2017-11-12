namespace BibleTraining.Api.Person
{
    using FluentValidation;

    public class UpdatePersonIntegrity : AbstractValidator<UpdatePerson>
    {
        public UpdatePersonIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new PersonDataIntegrity());
        }

        private class PersonDataIntegrity : AbstractValidator<PersonData>
        {
            public PersonDataIntegrity()
            {
                RuleFor(x => x.Id)
                    .NotNull();
                RuleFor(x => x.FirstName)
                    .NotEmpty();
                RuleFor(x => x.LastName)
                    .NotEmpty();
                RuleFor(x => x.Gender)
                    .NotEmpty();
            }
        }
    }
}
