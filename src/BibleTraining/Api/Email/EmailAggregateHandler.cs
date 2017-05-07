namespace BibleTraining.Api.Email
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
    public class EmailAggregateHandler :
        IAsyncRequestHandler<CreateEmail, EmailData>,
        IAsyncRequestHandler<GetEmails, EmailResult>,
        IAsyncRequestHandler<UpdateEmail, EmailData>,
        IRequestMiddleware<UpdateEmail, EmailData>,
        IAsyncRequestHandler<RemoveEmail, EmailData>,
        IRequestMiddleware<RemoveEmail, EmailData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;
        private readonly DateTime _now;

        public Email Email { get; set; }

        public EmailAggregateHandler(IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
            _now        = DateTime.Now;
        }

        #region Create Email

        public async Task<EmailData> Handle(CreateEmail message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var email = new Email().Map(message.Resource);
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

        #endregion

        #region Get Email

        public async Task<EmailResult> Handle(GetEmails message)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var emails = (await _repository.FindAsync(new GetEmailsById(message.Ids)))
                    .Select(x => new EmailData().Map(x)).ToArray();

                return new EmailResult
                {
                    Emails = emails
                };
            }
        }

        #endregion

        #region Update Email

        public async Task<EmailData> Apply(UpdateEmail request, Func<UpdateEmail, Task<EmailData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var data = request.Resource;
                if (Email == null && data != null)
                {
                    Email = await _repository.FetchByIdAsync<Email>(data.Id);
                    Env.Use(Email);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = Email?.RowVersion;
                return result;
            }
        }

        public Task<EmailData> Handle(UpdateEmail request)
        {
            Email.Map(request.Resource);

            return Task.FromResult(new EmailData
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove Email

        public async Task<EmailData> Apply(
            RemoveEmail request, Func<RemoveEmail, Task<EmailData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if (Email == null && resource != null)
                {
                    Email = await _repository.FetchByIdAsync<Email>(resource.Id);
                    Env.Use(Email);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

        public Task<EmailData> Handle(RemoveEmail request)
        {
            _repository.Context.Remove(Email);

            return Task.FromResult(new EmailData
            {
                Id         = Email.Id,
                RowVersion = Email.RowVersion
            });
        }

        #endregion

        #region Mapping



        #endregion
    }
}