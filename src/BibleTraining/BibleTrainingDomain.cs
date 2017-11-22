namespace BibleTraining
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;
    using Common.Logging;
    using Entities;
    using Highway.Data;
    using Highway.Data.EventManagement.Interfaces;
    using Highway.Data.Interceptors.Events;

    public interface IBibleTrainingDomain : IDomain
    {
    }

    public class BibleTrainingDomain : IBibleTrainingDomain
    {
        public string ConnectionString { get; } = nameof(BibleTrainingDomain);

        public IMappingConfiguration Mappings { get; set; }
        public IContextConfiguration Context  { get; set; }
        public List<IInterceptor>    Events   { get; set; }
    }

    public class BibleTrainingDomainContext : DomainContext<IBibleTrainingDomain>, IDomainContext<IDomain>
    {
        public BibleTrainingDomainContext(IBibleTrainingDomain domain) : base(domain)
        {
        }

        public override Task<int> CommitAsync()
        {
            var added = ChangeTracker.Entries<Entity>()
                .Where(x => x.State == EntityState.Added)
                .ToArray();

            var modified = ChangeTracker.Entries<Entity>()
                .Where(x => x.State == EntityState.Modified)
                .ToArray();

            foreach (var entity in added)
                entity.Entity.Created = entity.Entity.Modified = DateTime.Now;

            foreach (var entity in modified)
                entity.Entity.Modified = DateTime.Now;

            return base.CommitAsync();
        }
    }
}
