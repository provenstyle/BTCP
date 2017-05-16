namespace BibleTraining.Web.UI.Features.Search
{
    using System.Linq;
    using DataTables.AspNet.Core;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;

    public class EmailsController
        : DataTablesSearchController<Email, IBibleTrainingDomain>
    {
        public EmailsController(IRepository<IBibleTrainingDomain> repository)
            :base(repository)
        {
        }

        protected override IQueryable<Email> DefaultSort(IQueryable<Email> queryable)
        {
            return queryable.OrderBy(x => x.Address);
        }

        protected override IQueryable<Email> SortColumn(IQueryable<Email> queryable, IColumn column)
        {
            if(column.Is(nameof(Email.Address)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Address)
                    : queryable.OrderBy(x => x.Address);

            return queryable;
        }

        protected override IQueryable<Email> SearchAllColumns(IQueryable<Email> queryable, ISearch search)
        {
             return queryable.Where(x =>
                x.Address.Contains(search.Value));
        }

        protected override IQueryable<Email> FilterColumn(IQueryable<Email> queryable, IColumn column)
        {
            if(column.Is(nameof(Email.Address)))
                return queryable.Where(x => x.Address.Contains(column.Search.Value));
            return queryable;
        }
    }
}
