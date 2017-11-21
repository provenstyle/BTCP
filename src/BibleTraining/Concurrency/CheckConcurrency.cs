namespace BibleTraining.Concurrency
{
    using System.Data.Entity.Core;
    using System.Linq;
    using Api;
    using FluentValidation;
    using Improving.Highway.Data.Scope;
    using Miruken;
    using Miruken.Callback;
    using Miruken.Mediate;
    using Miruken.Validate.FluentValidation;

    public abstract class CheckConcurrency<TEntity, TRes>
        : CheckConcurrency<TEntity, TRes, UpdateResource<TRes, int?>>
        where TEntity : class,IEntity, IRowVersioned
        where TRes : Resource<int?>
    {
    }

    public abstract class CheckConcurrency<TEntity, TRes, TAction>
        : AbstractValidator<TAction>
        where TEntity : class, IEntity, IRowVersioned
        where TAction : UpdateResource<TRes, int?>
        where TRes : Resource<int?>
    {
        protected CheckConcurrency()
        {
            RuleFor(c => c.Resource)
                .NotNull()
                .OverridePropertyName(typeof(TEntity).Name);

            RuleFor(c => c.Resource)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .WithComposer(BeExpectedVersion)
                .WithName(typeof(TEntity).Name)
                .WithMessage(x => $"{typeof(TEntity).Name} is in an inconsitent state.");
        }

        private static bool BeExpectedVersion(TAction action, TRes res, IHandler composer)
        {
            var entity = composer.Proxy<IStash>().Get<TEntity>();

            if (entity == null || action.Resource?.RowVersion == null)
                return true;

            var resource = action.Resource;

            if (entity.Id != resource.Id)
                return false;

            if (resource.RowVersion == null ||
                resource.RowVersion.SequenceEqual(entity.RowVersion))
                return true;

            throw new OptimisticConcurrencyException(
                $"Concurrency exception detected for {entity.GetType()} with id {entity.Id}.");
        }
    }
}

