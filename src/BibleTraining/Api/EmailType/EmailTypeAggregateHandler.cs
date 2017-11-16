namespace BibleTraining.Api.EmailType
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
    using Queries;

    public class EmailTypeAggregateHandler : PipelineHandler,
        IMiddleware<UpdateEmailType, EmailTypeData>,
        IMiddleware<RemoveEmailType, EmailTypeData>
    {
        public int? Order { get; set; } = Stage.Validation - 1;

        private readonly IRepository<IBibleTrainingDomain> _repository;

        public EmailTypeAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public async Task<EmailType> EmailType(int? id, IHandler composer)
        {
            return await composer.Proxy<IStash>().GetOrPut(async () =>
                (await _repository.FindAsync(new GetEmailTypesById(id)))
                    .FirstOrDefault());
        }

        public async Task<EmailTypeData> Begin(int? id, IHandler composer, NextDelegate<Task<EmailTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var emailType = await EmailType(id, composer);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = emailType?.RowVersion;
                return result;
            }
        }

        [Mediates]
        public async Task<EmailTypeData> Create(CreateEmailType message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var emailType = composer.Proxy<IMapping>().Map<EmailType>(message.Resource);
                emailType.Created = DateTime.Now;
                _repository.Context.Add(emailType);

                var data = new EmailTypeData();
                await scope.SaveChangesAsync((dbScope, count) =>
                {
                    data.Id = emailType.Id;
                    data.RowVersion = emailType.RowVersion;
                });

                return data;
            }
        }

        [Mediates]
        public async Task<EmailTypeResult> Get(GetEmailTypes message, IHandler composer)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var emailTypes = (await _repository.FindAsync(new GetEmailTypesById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => composer.Proxy<IMapping>().Map<EmailTypeData>(x)).ToArray();

                return new EmailTypeResult
                {
                    EmailTypes = emailTypes
                };
            }
        }

        public async Task<EmailTypeData> Next(UpdateEmailType request, MethodBinding method, IHandler composer, NextDelegate<Task<EmailTypeData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<EmailTypeData> Update(UpdateEmailType request, IHandler composer)
        {
            var emailType = await EmailType(request.Resource.Id, composer);
            composer.Proxy<IMapping>()
                .MapInto(request.Resource, emailType);

            return new EmailTypeData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<EmailTypeData> Next(RemoveEmailType request, MethodBinding method, IHandler composer, NextDelegate<Task<EmailTypeData>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<EmailTypeData> Remove(RemoveEmailType request, IHandler composer)
        {
            var emailType = await EmailType(request.Resource.Id, composer);
            _repository.Context.Remove(emailType);

            return new EmailTypeData
            {
                Id         = emailType.Id,
                RowVersion = emailType.RowVersion
            };
        }
    }
}
