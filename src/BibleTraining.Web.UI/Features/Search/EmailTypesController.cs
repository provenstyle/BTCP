namespace BibleTraining.Web.UI.Features.Search
{
    using System.Linq;
    using DataTables.AspNet.Core;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;

    public class EmailTypesController
        : DataTablesSearchController<EmailType, IBibleTrainingDomain>
    {
        public EmailTypesController(IRepository<IBibleTrainingDomain> repository)
            :base(repository)
        {
        }

        protected override IQueryable<EmailType> DefaultSort(IQueryable<EmailType> queryable)
        {
            return queryable.OrderBy(x => x.Name);
        }

        protected override IQueryable<EmailType> SortColumn(IQueryable<EmailType> queryable, IColumn column)
        {
            if(column.Is(nameof(EmailType.Name)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Name)
                    : queryable.OrderBy(x => x.Name);

            return queryable;
        }

        protected override IQueryable<EmailType> SearchAllColumns(IQueryable<EmailType> queryable, ISearch search)
        {
             return queryable.Where(x =>
                x.Name.Contains(search.Value));
        }

        protected override IQueryable<EmailType> FilterColumn(IQueryable<EmailType> queryable, IColumn column)
        {
            if(column.Is(nameof(EmailType.Name)))
                return queryable.Where(x => x.Name.Contains(column.Search.Value));
            return queryable;
        }
    }
}
