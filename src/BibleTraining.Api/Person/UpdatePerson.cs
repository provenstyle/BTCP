namespace BibleTraining.Api.Person
{
    using Improving.MediatR;

    public class UpdatePerson : UpdateResource<PersonData, int?>
    {
        public UpdatePerson()
        {
        }

        public UpdatePerson(PersonData person)
            : base(person)
        {
        }
    }
}