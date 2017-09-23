namespace BibleTraining.Api.Person
{
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