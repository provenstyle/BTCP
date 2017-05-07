namespace BibleTraining.Api.Person
{
    using FluentValidation;

    public class CreatePersonIntegrity : AbstractValidator<CreatePerson>
    {
        public CreatePersonIntegrity()
        {
            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new PersonDataIntegrity());
        }

        private class PersonDataIntegrity : AbstractValidator<PersonData>
        {
            public PersonDataIntegrity()
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty();

                RuleFor(x => x.LastName)
                    .NotEmpty();

                RuleFor(x => x.Gender)
                    .NotNull();
            }
        }
    }
}