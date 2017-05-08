namespace BibleTraining.Api.EmailType
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
    public class EmailTypeAggregateHandler :
        IAsyncRequestHandler<CreateEmailType, EmailTypeData>,
        IAsyncRequestHandler<GetEmailTypes, EmailTypeResult>,
        IAsyncRequestHandler<UpdateEmailType, EmailTypeData>,
        IRequestMiddleware<UpdateEmailType, EmailTypeData>,
        IAsyncRequestHandler<RemoveEmailType, EmailTypeData>,
        IRequestMiddleware<RemoveEmailType, EmailTypeData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;
        private readonly DateTime _now;

        public EmailType EmailType { get; set; }

        public EmailTypeAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
            _now = DateTime.Now;
        }

        #region Create EmailType

        public async Task<EmailTypeData> Handle(CreateEmailType message)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var contactType = new EmailType().Map(message.Resource);
                contactType.Created = _now;

                _repository.Context.Add(contactType);

                var data = new EmailTypeData();

                await scope.SaveChangesAsync((dbScope, count) =>
                                                 {
                                                     data.Id = contactType.Id;
                                                     data.RowVersion = contactType.RowVersion;
                                                 });

                return data;
            }
        }

        #endregion

        #region Get EmailType

        public async Task<EmailTypeResult> Handle(GetEmailTypes message)
        {
            using (_repository.Scopes.CreateReadOnly())
            {
                var contactTypes = (await _repository.FindAsync(new GetEmailTypesById(message.Ids)
                {
                    KeyProperties = message.KeyProperties
                })).Select(x => new EmailTypeData().Map(x)).ToArray();

                return new EmailTypeResult
                {
                    EmailTypes = contactTypes
                };
            }
        }

        #endregion

        #region Update EmailType

        public async Task<EmailTypeData> Apply(UpdateEmailType request, Func<UpdateEmailType, Task<EmailTypeData>> next)
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

        public Task<EmailTypeData> Handle(UpdateEmailType request)
        {
            EmailType.Map(request.Resource);

            return Task.FromResult(new EmailTypeData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove EmailType

        public async Task<EmailTypeData> Apply(
            RemoveEmailType request, Func<RemoveEmailType, Task<EmailTypeData>> next)
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

        public Task<EmailTypeData> Handle(RemoveEmailType request)
        {
            _repository.Context.Remove(EmailType);

            return Task.FromResult(new EmailTypeData
            {
                Id = EmailType.Id,
                RowVersion = EmailType.RowVersion
            });
        }

        #endregion
    }
}