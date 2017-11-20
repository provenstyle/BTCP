namespace BibleTraining.Api.Person
{
    public class UpdatePerson :
        UpdateResource<PersonData, int?>,
        IValidateCreateUpdatePerson
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