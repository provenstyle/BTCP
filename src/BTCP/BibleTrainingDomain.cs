namespace BibleTraining
{
    using System.Collections.Generic;
    using Common.Logging;
    using Highway.Data;
    using Highway.Data.EventManagement.Interfaces;

    public interface IBibleTrainingDomain : IDomain
    {
    }

    public class BibleStudyDomain : IBibleTrainingDomain
    {
        public string ConnectionString { get; } = nameof(BibleStudyDomain);

        public IMappingConfiguration Mappings { get; set; }
        public IContextConfiguration Context  { get; set; }
        public List<IInterceptor>    Events   { get; set; }
    }

    public class BibleStudyDomainContext : DomainContext<IBibleTrainingDomain>, IDomainContext<IDomain>
    {
        public BibleStudyDomainContext(IBibleTrainingDomain domain) : base(domain)
        {
        }

        public BibleStudyDomainContext(IBibleTrainingDomain domain, ILog logger) : base(domain, logger)
        {
        }
    }
}
