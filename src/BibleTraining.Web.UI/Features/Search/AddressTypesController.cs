namespace BibleTraining.Web.UI.Features.Search
{
    using System.Linq;
    using DataTables.AspNet.Core;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;

    public class AddressTypesController
        : DataTablesSearchController<AddressType, IBibleTrainingDomain>
    {
        public AddressTypesController(IRepository<IBibleTrainingDomain> repository)
            :base(repository)
        {
        }

        protected override IQueryable<AddressType> DefaultSort(IQueryable<AddressType> queryable)
        {
            return queryable.OrderBy(x => x.Name);
        }

        protected override IQueryable<AddressType> SortColumn(IQueryable<AddressType> queryable, IColumn column)
        {
            if(column.Is(nameof(AddressType.Name)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Name)
                    : queryable.OrderBy(x => x.Name);

            return queryable;
        }

        protected override IQueryable<AddressType> SearchAllColumns(IQueryable<AddressType> queryable, ISearch search)
        {
             return queryable.Where(x =>
                x.Name.Contains(search.Value));
        }

        protected override IQueryable<AddressType> FilterColumn(IQueryable<AddressType> queryable, IColumn column)
        {
            if(column.Is(nameof(AddressType.Name)))
                return queryable.Where(x => x.Name.Contains(column.Search.Value));
            return queryable;
        }
    }
}
