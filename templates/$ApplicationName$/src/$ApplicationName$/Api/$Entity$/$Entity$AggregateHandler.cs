namespace $ApplicationName$.Api.$Entity$
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

    public class $Entity$AggregateHandler : PipelineHandler,
        IMiddleware<Update$Entity$, $Entity$Data>,
        IMiddleware<Remove$Entity$, $Entity$Data>
    {
        public int? Order { get; set; } = Stage.Validation - 1;

        private readonly IRepository<$DataDomain$> _repository;

        public $Entity$AggregateHandler(IRepository<$DataDomain$> repository)
        {
            _repository = repository;
        }

        public async Task<$Entity$> $Entity$(int? id, IHandler composer)
        {
            return await composer.Proxy<IStash>().GetOrPut(async () =>
                (await _repository.FindAsync(new Get$EntityPlural$ById(id)))
                    .FirstOrDefault());
        }

        public async Task<$Entity$Data> Begin(int? id, IHandler composer, NextDelegate<Task<$Entity$Data>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var $entityLowercase$ = await $Entity$(id, composer);
                var result = await next();
                await scope.SaveChangesAsync();

                result.RowVersion = $entityLowercase$?.RowVersion;
                return result;
            }
        }

        [Mediates]
        public async Task<$Entity$Data> Create(Create$Entity$ message, IHandler composer)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var $entityLowercase$ = composer.Proxy<IMapping>().Map<$Entity$>(message.Resource);
                $entityLowercase$.Created = DateTime.Now;

                _repository.Context.Add($entityLowercase$);

                var data = new $Entity$Data();

                await scope.SaveChangesAsync((dbScope, count) =>
                {
                    data.Id = $entityLowercase$.Id;
                    data.RowVersion = $entityLowercase$.RowVersion;
                });

                return data;
            }
        }

        [Mediates]
        public async Task<$Entity$Result> Get(Get$EntityPlural$ message, IHandler composer)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var $entityPluralLowercase$ = (await _repository.FindAsync(new Get$EntityPlural$ById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => composer.Proxy<IMapping>().Map<$Entity$Data>(x)).ToArray();

                return new $Entity$Result
                {
                    $EntityPlural$ = $entityPluralLowercase$
                };
            }
        }

        public async Task<$Entity$Data> Next(Update$Entity$ request, MethodBinding method, IHandler composer, NextDelegate<Task<$Entity$Data>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<$Entity$Data> Update(Update$Entity$ request, IHandler composer)
        {
            var $entityLowercase$ = await $Entity$(request.Resource.Id, composer);
            composer.Proxy<IMapping>()
                .MapInto(request.Resource, $entityLowercase$);

            return new $Entity$Data
            {
                Id = request.Resource.Id
            };
        }

        public async Task<$Entity$Data> Next(Remove$Entity$ request, MethodBinding method, IHandler composer, NextDelegate<Task<$Entity$Data>> next)
        {
            return await Begin(request.Resource.Id, composer, next);
        }

        [Mediates]
        public async Task<$Entity$Data> Remove(Remove$Entity$ request, IHandler composer)
        {
            var $entityLowercase$ = await $Entity$(request.Resource.Id, composer);
            _repository.Context.Remove($entityLowercase$);

            return new $Entity$Data
            {
                Id         = $entityLowercase$.Id,
                RowVersion = $entityLowercase$.RowVersion
            };
        }
    }
}