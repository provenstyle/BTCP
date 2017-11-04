namespace BibleTraining.Api.Email
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BibleTraining;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Callback.Policy;
    using Miruken.Map;
    using Miruken.Mediate;
    using Queries;

    public class EmailAggregateHandler : PipelineHandler,
        IGlobalMiddleware<UpdateEmail, EmailData>,
        IGlobalMiddleware<RemoveEmail, EmailData>
    {

        public int? Order { get; set; } = Stage.Validation - 1;
        private readonly IRepository<IBibleTrainingDomain> _repository;
        private readonly DateTime _now;

        public EmailAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
            _now        = DateTime.Now;
        }

        public async Task<Email> Email(int? id, IHandler composer)
        {
            return await composer.Proxy<IStash>().GetOrPut(async () =>
                (await _repository.FindAsync(new GetEmailsById(id)))
                    .FirstOrDefault());
        }

        public async Task<EmailData> Begin(int? id, IHandler composer, NextDelegate<Task<EmailData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var email = await Email(id, composer);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = email?.RowVersion;
                return result;
            }
        }

        public async Task<EmailData> Next(UpdateEmail callback, MethodBinding method, IHandler composer, NextDelegate<Task<EmailData>> next)
        {
            return await Begin(callback.Resource.Id, composer, next);
        }

        public async Task<EmailData> Next(RemoveEmail callback, MethodBinding method, IHandler composer, NextDelegate<Task<EmailData>> next)
        {
            return await Begin(callback.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<EmailData> Create(CreateEmail message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var email = composer.Proxy<IMapping>()
                    .Map<Email>(message.Resource);

                email.Created = _now;

                _repository.Context.Add(email);

                var data = new EmailData();

                await scope.SaveChangesAsync((dbScope, count) =>
                 {
                     data.Id = email.Id;
                     data.RowVersion = email.RowVersion;
                 });

                return data;
            }
        }

        [Mediates]
        public async Task<EmailResult> Get(GetEmails message, IHandler composer)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var emails = (await _repository.FindAsync(new GetEmailsById(message.Ids)))
                    .Select(x => composer.Proxy<IMapping>().Map<EmailData>(x)).ToArray();

                return new EmailResult
                {
                    Emails = emails
                };
            }
        }

        [Mediates]
        public async Task<EmailData> Update(UpdateEmail request, IHandler composer)
        {
            var email = await Email(request.Resource.Id, composer);
            composer.Proxy<IMapping>()
                    .MapInto(request.Resource, email);

            return new EmailData
            {
                Id = request.Resource.Id
            };
        }

        [Mediates]
        public async Task<EmailData> Remove(RemoveEmail request, IHandler composer)
        {
            var email = await Email(request.Resource.Id, composer);
            _repository.Context.Remove(email);

            return new EmailData
            {
                Id         = email.Id,
                RowVersion = email.RowVersion
            };
        }
    }
}