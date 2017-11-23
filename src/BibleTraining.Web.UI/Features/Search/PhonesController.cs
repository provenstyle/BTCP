namespace BibleTraining.Web.UI.Features.Search
{
    using System.Linq;
    using DataTables.AspNet.Core;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;

    public class PhonesController
        : DataTablesSearchController<Phone, IBibleTrainingDomain>
    {
        public PhonesController(IRepository<IBibleTrainingDomain> repository)
            :base(repository)
        {
        }

        protected override IQueryable<Phone> DefaultSort(IQueryable<Phone> queryable)
        {
            return queryable.OrderBy(x => x.Number);
        }

        protected override IQueryable<Phone> SortColumn(IQueryable<Phone> queryable, IColumn column)
        {
            if(column.Is(nameof(Phone.Number)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Number)
                    : queryable.OrderBy(x => x.Number);

            return queryable;
        }

        protected override IQueryable<Phone> SearchAllColumns(IQueryable<Phone> queryable, ISearch search)
        {
             return queryable.Where(x =>
                x.Number.Contains(search.Value));
        }

        protected override IQueryable<Phone> FilterColumn(IQueryable<Phone> queryable, IColumn column)
        {
            if(column.Is(nameof(Phone.Number)))
                return queryable.Where(x => x.Number.Contains(column.Search.Value));
            return queryable;
        }
    }
}
