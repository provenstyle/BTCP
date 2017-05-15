namespace $ApplicationName$.Api.$Entity$ 
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Improving.Highway.Data.Scope.Repository;
    using Improving.MediatR;
    using Improving.MediatR.Pipeline;
    using MediatR;
    using Entity;

    [RelativeOrder(Stage.Validation - 1)]
    public class $Entity$AggregateHandler :
        IAsyncRequestHandler<Create$Entity$, $Entity$Data>,
        IAsyncRequestHandler<Get$EntityPlural$, $Entity$Result>,
        IAsyncRequestHandler<Update$Entity$, $Entity$Data>,
        IRequestMiddleware<Update$Entity$, $Entity$Data>,
        IAsyncRequestHandler<Remove$Entity$, $Entity$Data>,
        IRequestMiddleware<Remove$Entity$, $Entity$Data>
    {
        private readonly IRepository<$DataDomain$> _repository;

        public $Entity$ $Entity$ { get; set; }

        public $Entity$AggregateHandler(IRepository<$DataDomain$> repository)
        {
            _repository = repository;
        }

        #region Create $Entity$

        public async Task<$Entity$Data> Handle(Create$Entity$ message)
        {
            using(var scope = _repository.Scopes.Create())
            {
                var $entityLowercase$ = new $Entity$().Map(message.Resource);
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

        #endregion

        #region Get $Entity$

        public async Task<$Entity$Result> Handle(Get$EntityPlural$ message)
        {
            using(_repository.Scopes.CreateReadOnly())
            {
                var $entityPluralLowercase$ = (await _repository.FindAsync(new Get$EntityPlural$ById(message.Ids){
                    KeyProperties = message.KeyProperties
                })).Select(x => new $Entity$Data().Map(x)).ToArray();

                return new $Entity$Result
                {
                    $EntityPlural$ = $entityPluralLowercase$
                };
            }
        }

        #endregion

        #region Update $Entity$

        public async Task<$Entity$Data> Apply(Update$Entity$ request, Func<Update$Entity$, Task<$Entity$Data>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if ($Entity$ == null && resource != null)
                {
                    $Entity$ = (await _repository
                        .FindAsync(new Get$EntityPlural$ById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use($Entity$);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();

                result.RowVersion = $Entity$?.RowVersion;
                return result;
            }
        }

        public Task<$Entity$Data> Handle(Update$Entity$ request)
        {
            $Entity$.Map(request.Resource);

            return Task.FromResult(new $Entity$Data
            {
                Id = request.Resource.Id
            });
        }

        #endregion

        #region Remove $Entity$

        public async Task<$Entity$Data> Apply(
            Remove$Entity$ request, Func<Remove$Entity$, Task<$Entity$Data>> next)
        {
            using (var scope = _repository.Scopes.Create())
            {
                var resource = request.Resource;
                if ($Entity$ == null && resource != null)
                {
                    $Entity$ = (await _repository
                        .FindAsync(new Get$EntityPlural$ById(resource.Id)))
                        .FirstOrDefault();
                    Env.Use($Entity$);
                }

                var result = await next(request);
                await scope.SaveChangesAsync();
                return result;
            }
        }

        public Task<$Entity$Data> Handle(Remove$Entity$ request)
        {
            _repository.Context.Remove($Entity$);

            return Task.FromResult(new $Entity$Data
            {
                Id         = $Entity$.Id,
                RowVersion = $Entity$.RowVersion
            });
        }

        #endregion

    }
}