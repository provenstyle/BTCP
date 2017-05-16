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
            return queryable.OrderBy(x => x.Name);
        }

        protected override IQueryable<Phone> SortColumn(IQueryable<Phone> queryable, IColumn column)
        {
            if(column.Is(nameof(Phone.Name)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Name)
                    : queryable.OrderBy(x => x.Name);

            if(column.Is(nameof(Phone.Description)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Description)
                    : queryable.OrderBy(x => x.Description);

            return queryable;
        }

        protected override IQueryable<Phone> SearchAllColumns(IQueryable<Phone> queryable, ISearch search)
        {
             return queryable.Where(x =>
                x.Name.Contains(search.Value) ||
                x.Description.Contains(search.Value));
        }

        protected override IQueryable<Phone> FilterColumn(IQueryable<Phone> queryable, IColumn column)
        {
            if(column.Is(nameof(Phone.Name)))
                return queryable.Where(x => x.Name.Contains(column.Search.Value));
            if(column.Is(nameof(Phone.Description)))
                return queryable.Where(x => x.Description.Contains(column.Search.Value));
            return queryable;
        }
    }
}
