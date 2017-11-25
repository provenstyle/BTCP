namespace BibleTraining.Concurrency
{
    using FluentValidation.Validators;
    using System.Data.Entity.Core;
    using System.Linq;
    using Api;
    using FluentValidation;
    using Improving.Highway.Data.Scope;
    using Miruken;
    using Miruken.Mediate;
    using Miruken.Validate.FluentValidation;

    public abstract class CheckConcurrency<TEntity, TRes>
        : CheckConcurrency<TEntity, TRes, UpdateResource<TRes, int?>>
        where TEntity : class, IEntity, IRowVersioned
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
            RuleFor(c => c).Custom(BeExpectedVersion);
        }

        private static void BeExpectedVersion(TAction action, CustomContext context)
        {
            var resource = action.Resource;
            if (resource == null) return;

            var composer = context.ParentContext?.GetComposer();
            var entity   = composer?.Proxy<IStash>().TryGet<TEntity>();
            if (entity == null)
            {
                context.AddFailure(typeof(TEntity).Name,
                    $"{typeof(TEntity)} with id {resource.Id} not found.");
                return;
            }

            if (entity.Id != resource.Id)
            {
                context.AddFailure($"{entity.GetType()}.Id",
                    $"{entity.GetType()} has id {entity.Id} but expected id {resource.Id}.");
                return;
            }

            if (resource.RowVersion?.SequenceEqual(entity.RowVersion) != true)
                throw new OptimisticConcurrencyException(
                    $"Concurrency exception detected for {entity.GetType()} with id {entity.Id}.");
        }
    }
}

