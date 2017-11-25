namespace BibleTraining.Api.Email
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;
    using Miruken.Callback;
    using Miruken.Callback.Policy;
    using Miruken.Map;
    using Miruken.Mediate;
    using Queries;

    public class EmailAggregateHandler : PipelineHandler,
        IMiddleware<UpdateEmail, EmailData>,
        IMiddleware<RemoveEmail, EmailData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;
        private readonly DateTime _now;

        public EmailAggregateHandler(
            IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
            _now        = DateTime.Now;
        }

        public int? Order { get; set; } = Stage.Validation - 1;

        [Mediates]
        public async Task<EmailData> Create(CreateEmail request,
            StashOf<Person> person, StashOf<EmailType> emailType,
            StashOf<Email> emailStash, [Proxy]IMapping mapper)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var emailData = request.Resource;
                var email     = mapper.Map<Email>(request.Resource);
                _repository.Context.Add(email);
                emailStash.Value = email;

                if (emailData.PersonId.HasValue)
                    email.PersonId = emailData.PersonId.Value;
                else
                    email.Person = person.Value;

                if (emailData.EmailTypeId.HasValue)
                    email.EmailTypeId = emailData.EmailTypeId.Value;
                else
                    email.EmailType = emailType.Value;

                var data = new EmailData();
                await scope.SaveChangesAsync((dbScope, count) =>
                 {
                     data.Id         = email.Id;
                     data.RowVersion = email.RowVersion;
                 });

                return data;
            }
        }

        [Mediates]
        public async Task<EmailResult> Get(
            GetEmails message, [Proxy]IMapping mapper)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var emails = (await _repository.FindAsync(
                    new GetEmailsById(message.Ids)))
                    .Select(x => mapper.Map<EmailData>(x))
                    .ToArray();

                return new EmailResult
                {
                    Emails = emails
                };
            }
        }

        public async Task<EmailData> Next(
            UpdateEmail callback, MethodBinding method,
            IHandler composer, NextDelegate<Task<EmailData>> next)
        {
            return await Begin(callback.Resource.Id,
                new StashOf<Email>(composer), next);
        }

        [Mediates]
        public async Task<EmailData> Update(
            UpdateEmail request, StashOf<Email> emailStash,
            [Proxy]IMapping mapper)
        {
            var email = await Email(request.Resource.Id, emailStash);
            mapper.MapInto(request.Resource, email);

            return new EmailData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<EmailData> Next(
            RemoveEmail callback, MethodBinding method,
            IHandler composer, NextDelegate<Task<EmailData>> next)
        {
            return await Begin(callback.Resource.Id,
                new StashOf<Email>(composer), next);
        }

        [Mediates]
        public async Task<EmailData> Remove(
            RemoveEmail request, StashOf<Email> emailStash)
        {
            var email = await Email(request.Resource.Id, emailStash);
            _repository.Context.Remove(email);

            return new EmailData
            {
                Id         = email.Id,
                RowVersion = email.RowVersion
            };
        }

        protected async Task<EmailData> Begin(
            int? id, StashOf<Email> emailStash,
            NextDelegate<Task<EmailData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var email  = await Email(id, emailStash);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = email?.RowVersion;
                return result;
            }
        }

        protected Task<Email> Email(int? id, StashOf<Email> email)
        {
            return email.GetOrPut(async _ =>
                (await _repository.FindAsync(new GetEmailsById(id)))
                .FirstOrDefault());
        }
    }
}