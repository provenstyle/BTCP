namespace BibleTraining.Web.UI.Features.Search
{
    using System.Linq;
    using DataTables.AspNet.Core;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;

    public class AddressesController
        : DataTablesSearchController<Address, IBibleTrainingDomain>
    {
        public AddressesController(IRepository<IBibleTrainingDomain> repository)
            :base(repository)
        {
        }

        protected override IQueryable<Address> DefaultSort(IQueryable<Address> queryable)
        {
            return queryable.OrderBy(x => x.Name);
        }

        protected override IQueryable<Address> SortColumn(IQueryable<Address> queryable, IColumn column)
        {
            if(column.Is(nameof(Address.Name)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Name)
                    : queryable.OrderBy(x => x.Name);

            if(column.Is(nameof(Address.Description)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Description)
                    : queryable.OrderBy(x => x.Description);

            return queryable;
        }

        protected override IQueryable<Address> SearchAllColumns(IQueryable<Address> queryable, ISearch search)
        {
             return queryable.Where(x =>
                x.Name.Contains(search.Value) ||
                x.Description.Contains(search.Value));
        }

        protected override IQueryable<Address> FilterColumn(IQueryable<Address> queryable, IColumn column)
        {
            if(column.Is(nameof(Address.Name)))
                return queryable.Where(x => x.Name.Contains(column.Search.Value));
            if(column.Is(nameof(Address.Description)))
                return queryable.Where(x => x.Description.Contains(column.Search.Value));
            return queryable;
        }
    }
}
