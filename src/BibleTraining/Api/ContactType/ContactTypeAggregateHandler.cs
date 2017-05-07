namespace BibleTraining.Api.ContactType
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
    public class ContactTypeAggregateHandler :
        IAsyncRequestHandler<CreateContactType, ContactTypeData>,
        IAsyncRequestHandler<GetContactTypes, ContactTypeResult>,
        IAsyncRequestHandler<UpdateContactType, ContactTypeData>,
        IRequestMiddleware<UpdateContactType, ContactTypeData>,
        IAsyncRequestHandler<RemoveContactType, ContactTypeData>,
        IRequestMiddleware<RemoveContactType, ContactTypeData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;
        private readonly DateTime _now;

        public EmailType EmailType { get; set; }

        public ContactTypeAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
            _now = DateTime.Now;
        }

        #region Create ContactType

        public async Task<ContactTypeData> Handle(CreateContactType message)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var contactType = new EmailType().Map(message.Resource);
                contactType.Created = _now;

                _repository.Context.Add(contactType);

                var data = new ContactTypeData();

                await scope.SaveChangesAsync((dbScope, count) =>
                                                 {
                                                     data.Id = contactType.Id;
                                                     data.RowVersion = contactType.RowVersion;
                                                 });

                return data;
            }
        }

        #endregion

        #region Get ContactType

        public async Task<ContactTypeResult> Handle(GetContactTypes message)
        {
            using (_repository.Scopes.CreateReadOnly())
            {
                var contactTypes = (await _repository.FindAsync(new GetContactTypesById(message.Ids)
                {
                    KeyProperties = message.KeyProperties
                })).Select(x => new ContactTypeData().Map(x)).ToArray();

                return new ContactTypeResult
                {
                    ContactTypes = contactTypes
                };
            }
        }

        #endregion

        #region Update ContactType

        public async Task<ContactTypeData> Apply(UpdateContactType request, Func<UpdateContactType, Task<ContactTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var data = request.Resource;
                if (EmailType == null && data != null)
                {
                    EmailType = await _repository.FetchByIdAsync<EmailType>(data.Id);
                    Env.Use(EmailType);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = EmailType?.RowVersion;
                return result;
            }
        }

        public Task<ContactTypeData> Handle(UpdateContactType request)
        {
            EmailType.Map(request.Resource);

            return Task.FromResult(new ContactTypeData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove ContactType

        public async Task<ContactTypeData> Apply(
            RemoveContactType request, Func<RemoveContactType, Task<ContactTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (EmailType == null && resource != null)
                {
                    EmailType = await _repository.FetchByIdAsync<EmailType>(resource.Id);
                    Env.Use(EmailType);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

        public Task<ContactTypeData> Handle(RemoveContactType request)
        {
            _repository.Context.Remove(EmailType);

            return Task.FromResult(new ContactTypeData
            {
                Id = EmailType.Id,
                RowVersion = EmailType.RowVersion
            });
        }

        #endregion
    }
}