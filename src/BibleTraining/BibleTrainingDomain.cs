namespace BibleTraining
{
    using System.Collections.Generic;
    using Common.Logging;
    using Highway.Data;
    using Highway.Data.EventManagement.Interfaces;

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

        public BibleTrainingDomainContext(IBibleTrainingDomain domain, ILog logger) : base(domain, logger)
        {
        }
    }
}
