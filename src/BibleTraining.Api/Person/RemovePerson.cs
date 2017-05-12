namespace BibleTraining.Api.Person
{
    using Improving.MediatR;

    public class RemovePerson : UpdateResource<PersonData, int?>
    {
        public RemovePerson()
        {
        }

        public RemovePerson(PersonData person)
            : base(person)
        {
        }
    }
}