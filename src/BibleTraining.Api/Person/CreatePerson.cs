namespace BibleTraining.Api.Person
{
    using Improving.MediatR;

    public class CreatePerson : ResourceAction<PersonData, int?>
    {
        public CreatePerson()
        {
        }

        public CreatePerson(PersonData person)
            : base (person)
        {
        }
    }
}