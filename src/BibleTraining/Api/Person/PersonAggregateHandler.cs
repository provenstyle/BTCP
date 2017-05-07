namespace BibleTraining.Api.Person
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Improving.MediatR;
    using Improving.MediatR.Pipeline;
    using MediatR;
    using Queries;

    [RelativeOrder(Stage.Validation - 1)]
    public class PersonAggregateHandler :
        IAsyncRequestHandler<CreatePerson, PersonData>,
        IAsyncRequestHandler<GetPeople, PersonResult>,
        IAsyncRequestHandler<UpdatePerson, PersonData>,
        IRequestMiddleware<UpdatePerson, PersonData>,
        IAsyncRequestHandler<RemovePerson, PersonData>,
        IRequestMiddleware<RemovePerson, PersonData>
    {
        private readonly IRepository<BibleTrainingDomain> _repository;
        private readonly DateTime _now;

        public Person Person { get; set; }

        public PersonAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
            _now        = DateTime.Now;
        }

        #region Create Person

        public async Task<PersonData> Handle(CreatePerson message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var person = Map(new Person(), message.Resource);
                person.Created = _now;

                _repository.Context.Add(person);

                var data = new PersonData();

                await scope.SaveChangesAsync((dbScope, count) =>
                 {
                     data.Id = person.Id;
                     data.RowVersion = person.RowVersion;
                 });

                return data;
            }
        }

        #endregion

        #region Get Person

        public async Task<PersonResult> Handle(GetPeople message)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var people = (await _repository.FindAsync(new GetPeopleById(message.Ids)))
                    .Select(x => Map(new PersonData(), x)).ToArray();

                return new PersonResult
                {
                    People = people
                };
            }
        }

        #endregion

        #region Update Person

        public async Task<PersonData> Apply(UpdatePerson request, Func<UpdatePerson, Task<PersonData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var data = request.Resource;
                if (Person == null && data != null)
                {
                    Person = await _repository.FetchByIdAsync<Person>(data.Id);
                    Env.Use(Person);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = Person?.RowVersion;
                return result;
            }
        }

        public Task<PersonData> Handle(UpdatePerson request)
        {
            Map(Person, request.Resource);

            return Task.FromResult(new PersonData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove Person

        public async Task<PersonData> Apply(
            RemovePerson request, Func<RemovePerson, Task<PersonData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Person == null && resource != null)
                {
                    Person = await _repository.FetchByIdAsync<Person>(resource.Id);
                    Env.Use(Person);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

        public Task<PersonData> Handle(RemovePerson request)
        {
            _repository.Context.Remove(Person);

            return Task.FromResult(new PersonData
            {
                Id         = Person.Id,
                RowVersion = Person.RowVersion
            });
        }

        #endregion


        #region Mapping

        public Person Map(Person person, PersonData data)
        {
            if (data.FirstName != null)
                person.FirstName = data.FirstName;

            if (data.LastName != null)
                person.LastName = data.LastName;

            if (data.Bio != null)
                person.Bio = data.Bio;

            if (data.BirthDate.HasValue)
                person.BirthDate = data.BirthDate.Value;

            if (data.Gender.HasValue)
                person.Gender = data.Gender.Value;

            if (data.Image != null)
                person.Image = data.Image;

            if (data.CreatedBy != null)
                person.CreatedBy = data.CreatedBy;

            if (data.ModifiedBy != null)
                person.ModifiedBy = data.ModifiedBy;

            person.Modified = _now;

            return person;
        }

        public PersonData Map(PersonData data, Person person)
        {
            data.Id         = person.Id;
            data.FirstName  = person.FirstName;
            data.LastName   = person.LastName;
            data.Bio        = person.Bio;
            data.BirthDate  = person.BirthDate;
            data.Gender     = person.Gender;
            data.Image      = person.Image;

            data.RowVersion = person.RowVersion;
            data.CreatedBy  = person.CreatedBy;
            data.Created    = person.Created;
            data.ModifiedBy = person.ModifiedBy;
            data.Modified   = person.Modified;

            return data;
        }

        #endregion
    }
}