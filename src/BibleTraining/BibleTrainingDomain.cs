namespace BibleTraining
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
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
            BeforeSave += BibleTrainingDomainContext_BeforeSave;
        }

        private void BibleTrainingDomainContext_BeforeSave(object sender, Highway.Data.Interceptors.Events.BeforeSave e)
        {
            var added = ChangeTracker.Entries<Entity>().Where(x => x.State == EntityState.Added);
            foreach (var entity in added)
                entity.Entity.Created = entity.Entity.Modified = DateTime.Now;

            var modified = ChangeTracker.Entries<Entity>().Where(x => x.State == EntityState.Modified);
            foreach (var entity in modified)
                entity.Entity.Modified = DateTime.Now;
        }
    }

    public class BeforeSave : IInterceptor
    {
        public int Priority { get; }
    }
}
