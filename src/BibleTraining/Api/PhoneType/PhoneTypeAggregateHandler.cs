namespace BibleTraining.Api.PhoneType
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

    public class PhoneTypeAggregateHandler : PipelineHandler,
        IMiddleware<UpdatePhoneType, PhoneTypeData>,
        IMiddleware<RemovePhoneType, PhoneTypeData>
    {
        private readonly IRepository<IBibleTrainingDomain> _repository;

        public PhoneTypeAggregateHandler(
            IRepository<IBibleTrainingDomain> repository)
        {
            _repository = repository;
        }

        public int? Order { get; set; } = Stage.Validation - 1;

        [Mediates]
        public async Task<PhoneTypeData> Create(
            CreatePhoneType message, [Proxy]IMapping mapper)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var phoneType = mapper.Map<PhoneType>(message.Resource);
                phoneType.Created = DateTime.Now;

                _repository.Context.Add(phoneType);

                var data = new PhoneTypeData();

                await scope.SaveChangesAsync((dbScope, count) =>
                {
                    data.Id         = phoneType.Id;
                    data.RowVersion = phoneType.RowVersion;
                });

                return data;
            }
        }

        [Mediates]
        public async Task<PhoneTypeResult> Get(
            GetPhoneTypes message, [Proxy]IMapping mapper)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var phoneTypes = (await _repository.FindAsync(
                    new GetPhoneTypesById(message.Ids){
                        KeyProperties = message.KeyProperties
                    }))
                    .Select(x => mapper.Map<PhoneTypeData>(x))
                    .ToArray();

                return new PhoneTypeResult
                {
                    PhoneTypes = phoneTypes
                };
            }
        }

        public async Task<PhoneTypeData> Next(
            UpdatePhoneType request, MethodBinding method, 
            IHandler composer, NextDelegate<Task<PhoneTypeData>> next)
        {
            return await Begin(request.Resource.Id, 
                new StashOf<PhoneType>(composer), next);
        }

        [Mediates]
        public async Task<PhoneTypeData> Update(
            UpdatePhoneType request, StashOf<PhoneType> phoneTypeStash,
            [Proxy]IMapping mapper)
        {
            var phoneType = await PhoneType(request.Resource.Id, phoneTypeStash);
            mapper.MapInto(request.Resource, phoneType);

            return new PhoneTypeData
            {
                Id = request.Resource.Id
            };
        }

        public async Task<PhoneTypeData> Next(
            RemovePhoneType request, MethodBinding method,
            IHandler composer, NextDelegate<Task<PhoneTypeData>> next)
        {
            return await Begin(request.Resource.Id, 
                new StashOf<PhoneType>(composer), next);
        }

        [Mediates]
        public async Task<PhoneTypeData> Remove(
            RemovePhoneType request, StashOf<PhoneType> phoneTypeStash)
        {
            var phoneType = await PhoneType(request.Resource.Id, phoneTypeStash);
            _repository.Context.Remove(phoneType);

            return new PhoneTypeData
            {
                Id         = phoneType.Id,
                RowVersion = phoneType.RowVersion
            };
        }

        public async Task<PhoneTypeData> Begin(
            int? id, StashOf<PhoneType> phoneTypeStash,
            NextDelegate<Task<PhoneTypeData>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var phoneType = await PhoneType(id, phoneTypeStash);
                var result    = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = phoneType?.RowVersion;
                return result;
            }
        }

        protected Task<PhoneType> PhoneType(int? id, StashOf<PhoneType> phoneType)
        {
            return phoneType.GetOrPut(async _ =>
                (await _repository.FindAsync(new GetPhoneTypesById(id)))
                .FirstOrDefault());
        }
    }
}
