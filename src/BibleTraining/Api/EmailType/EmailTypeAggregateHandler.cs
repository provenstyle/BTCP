namespace BibleTraining.Api.EmailType
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken.Callback;
    using Miruken.Callback.Policy;
    using Miruken.Map;
    using Miruken.Mediate;
    using Queries;

    public class EmailTypeAggregateHandler : PipelineHandler,
        IMiddleware<UpdateEmailType, EmailTypeData>,
        IMiddleware<RemoveEmailType, EmailTypeData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public EmailTypeAggregateHandler(
            IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public int? Order { get; set; } = Stage.Validation - 1;

        [Mediates]
        public async Task<EmailTypeData> Create(
            CreateEmailType message, [Proxy]IMapping mapper)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var emailType = mapper.Map<EmailType>(message.Resource);
                _repository.Context.Add(emailType);

                var data = new EmailTypeData();
                await scope.SaveChangesAsync((dbScope, count) =>
                {
                    data.Id         = emailType.Id;
                    data.RowVersion = emailType.RowVersion;
                });

                return data;
            }
        }

        [Mediates]
        public async Task<EmailTypeResult> Get(
            GetEmailTypes message, [Proxy]IMapping mapper)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var emailTypes = (await _repository.FindAsync(
                    new GetEmailTypesById(message.Ids) {
                        KeyProperties = message.KeyProperties
                    }))
                    .Select(x => mapper.Map<EmailTypeData>(x))
                    .ToArray();

                return new EmailTypeResult
                {
                    EmailTypes = emailTypes
                };
            }
        }

        public async Task<EmailTypeData> Next(
            UpdateEmailType request, MethodBinding method, 
            IHandler composer, NextDelegate<Task<EmailTypeData>> next)
        {
            return await Begin(request.Resource.Id, 
                new StashOf<EmailType>(composer), next);
        }

        [Mediates]
        public async Task<EmailTypeData> Update(
            UpdateEmailType request, StashOf<EmailType> emailTypeStash,
            [Proxy]IMapping mapper)
        {
            var emailType = await EmailType(request.Resource.Id, emailTypeStash);
            mapper.MapInto(request.Resource, emailType);

            return new EmailTypeData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<EmailTypeData> Next(
            RemoveEmailType request, MethodBinding method, 
            IHandler composer, NextDelegate<Task<EmailTypeData>> next)
        {
            return await Begin(request.Resource.Id, 
                new StashOf<EmailType>(composer), next);
        }

        [Mediates]
        public async Task<EmailTypeData> Remove(
            RemoveEmailType request, StashOf<EmailType> emailTypeStash)
        {
            var emailType = await EmailType(request.Resource.Id, emailTypeStash);
            _repository.Context.Remove(emailType);

            return new EmailTypeData
            {
                Id         = emailType.Id,
                RowVersion = emailType.RowVersion
            };
        }

        protected async Task<EmailTypeData> Begin(
            int? id, StashOf<EmailType> emailTypeStash,
            NextDelegate<Task<EmailTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var emailType = await EmailType(id, emailTypeStash);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = emailType?.RowVersion;
                return result;
            }
        }

        protected Task<EmailType> EmailType(int? id, StashOf<EmailType> emailType)
        {
            return emailType.GetOrPut(async _ =>
                (await _repository.FindAsync(new GetEmailTypesById(id)))
                .FirstOrDefault());
        }
    }
}
