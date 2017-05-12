namespace BibleTraining.Api.Person
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Address;
    using BibleTraining;
    using Email;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Improving.MediatR;
    using Improving.MediatR.Concurrency;
    using Improving.MediatR.Pipeline;
    using MediatR;
    using Phone;
    using Queries;
    using Test._CodeGeneration;

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
        private readonly IMediator _mediator;
        private readonly DateTime _now;

        public Person Person { get; set; }

        public PersonAggregateHandler(IRepository<IBibleTrainingDomain> repository, IMediator mediator)
        {
            _repository = repository;
            _mediator   = mediator;
            _now        = DateTime.Now;
        }

        #region Create Person

        public async Task<PersonData> Handle(CreatePerson message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var person = new Person().Map(message.Resource);
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
                var people = (await _repository.FindAsync(new GetPeopleById(message.Ids)
                    {
                        IncludeEmails    = message.IncludeEmails,
                        IncludeAddresses = message.IncludeAddresses,
                        IncludePhones    = message.IncludePhones
                    }))
                    .Select(x => new PersonData().Map(x)).ToArray();

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
                var resource = request.Resource;
                if (Person == null && resource != null)
                {
                    Person = (await _repository
                        .FindAsync(new GetPeopleById(resource.Id)
                          {
                              IncludeEmails     = true,
                              IncludeAddresses  = true,
                              IncludePhones     = true
                          }))
                        .FirstOrDefault();
                    Env.Use(Person);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = Person?.RowVersion;
                return result;
            }
        }

        public async Task<PersonData> Handle(UpdatePerson request)
        {
            Person.Map(request.Resource);

            var relationships = new List<object>();

            var emails = request.Resource.Emails;
            if (emails != null)
            {
                var adds      = emails.Where(x => !x.Id.HasValue).ToArray();
                var updates   = emails.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes   = Person.Emails?.Where(x => !updateIds.Contains(x.Id)).ToArray();

                foreach (var add in adds)
                    relationships.Add(new CreateEmail(add));

                foreach (var update in updates)
                    relationships.Add(new UpdateEmail(update));

                foreach (var remove in removes)
                    relationships.Add(new RemoveEmail(new EmailData().Map(remove)));
            }

            var addresses = request.Resource.Addresses;
            if (addresses != null)
            {
                var adds      = addresses.Where(x => !x.Id.HasValue).ToArray();
                var updates   = addresses.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes   = Person.Addresses?.Where(x => !updateIds.Contains(x.Id)).ToArray();

                foreach (var add in adds)
                    relationships.Add(new CreateAddress(add));

                foreach (var update in updates)
                    relationships.Add(new UpdateAddress(update));

                foreach (var remove in removes)
                    relationships.Add(new RemoveAddress(new AddressData().Map(remove)));
            }

            var phones = request.Resource.Phones;
            if (phones != null)
            {
                var adds      = phones.Where(x => !x.Id.HasValue).ToArray();
                var updates   = phones.Where(x => x.Id.HasValue).ToArray();
                var updateIds = updates.Select(x => x.Id).ToArray();
                var removes   = Person.Phones?.Where(x => !updateIds.Contains(x.Id)).ToArray();

                foreach (var add in adds)
                    relationships.Add(new CreatePhone(add));

                foreach (var update in updates)
                    relationships.Add(new UpdatePhone(update));

                foreach (var remove in removes)
                    relationships.Add(new RemovePhone(new PhoneData().Map(remove)));
            }

            if (relationships.Any())
            {
                await _mediator.SendAsync(new Sequential
                {
                    Requests = relationships.ToArray()
                });
            }

            return new PersonData
            {
                Id = request.Resource.Id
            };
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
    }
}