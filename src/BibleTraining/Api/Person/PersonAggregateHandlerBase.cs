namespace BibleTraining.Api.Person
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Callback.Policy;
    using Miruken.Map;
    using Miruken.Mediate;
    using Miruken.Mediate.Schedule;
    using Queries;

    public abstract class PersonAggregateHandlerBase : PipelineHandler,
        IGlobalMiddleware<UpdatePerson, PersonData>,
        IGlobalMiddleware<RemovePerson, PersonData>
    {
        public int? Order { get; set; } = Stage.Validation - 1;

        protected readonly IRepository<IBibleTrainingDomain> _repository;

        protected PersonAggregateHandlerBase(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        protected abstract Task<Person> Person(int? id, IHandler composer);

        public async Task<PersonData> Begin(int? id, IHandler composer, NextDelegate<Task<PersonData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var person = await Person(id, composer);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = person?.RowVersion;
                return result;
            }
        }

        public virtual Task<object[]> GetUpdateRelationships(UpdatePerson request,
            Person person, IHandler composer)
        {
            return Task.FromResult(new object[] { });
        }

        [Mediates]
        public async Task<PersonData> Create(CreatePerson message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var person = composer.Proxy<IMapping>().Map<Person>(message.Resource);
                person.Created = DateTime.Now;

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

        [Mediates]
        public async Task<PersonResult> Get(GetPeople message, IHandler composer)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var people = (await _repository
                    .FindAsync(new GetPeopleById(message.Ids)
                      {
                         IncludePhones = message.IncludePhones,
                         IncludeEmails = message.IncludeEmails,
                         IncludeAddresses = message.IncludeAddresses
                      }))
                    .Select(x => composer.Proxy<IMapping>().Map<PersonData>(x)).ToArray();

                return new PersonResult
                {
                    People = people
                };
            }
        }

        public async Task<PersonData> Next(UpdatePerson request, MethodBinding method, IHandler composer, NextDelegate<Task<PersonData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }


        [Mediates]
        public async Task<PersonData> Update(UpdatePerson request, IHandler composer)
        {
            var person = await Person(request.Resource.Id, composer);

            var relationships = await GetUpdateRelationships(request, person, composer);

            composer.Proxy<IMapping>()
                .MapInto(request.Resource, person);

            if (relationships.Any())
            {
                await composer.Send(new Sequential
                {
                    Requests = relationships.ToArray()
                });
            }

            return new PersonData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<PersonData> Next(RemovePerson request, MethodBinding method, IHandler composer, NextDelegate<Task<PersonData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<PersonData> Remove(RemovePerson request, IHandler composer)
        {
            var person = await Person(request.Resource.Id, composer);
            _repository.Context.Remove(person);

            return new PersonData
            {
                Id         = person.Id,
                RowVersion = person.RowVersion
            };
        }
    }
}
