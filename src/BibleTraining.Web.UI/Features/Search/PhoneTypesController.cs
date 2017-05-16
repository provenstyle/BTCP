namespace BibleTraining.Web.UI.Features.Search
{
    using System.Linq;
    using DataTables.AspNet.Core;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;

    public class PhoneTypesController
        : DataTablesSearchController<PhoneType, IBibleTrainingDomain>
    {
        public PhoneTypesController(IRepository<IBibleTrainingDomain> repository)
            :base(repository)
        {
        }

        protected override IQueryable<PhoneType> DefaultSort(IQueryable<PhoneType> queryable)
        {
            return queryable.OrderBy(x => x.Name);
        }

        protected override IQueryable<PhoneType> SortColumn(IQueryable<PhoneType> queryable, IColumn column)
        {
            if(column.Is(nameof(PhoneType.Name)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Name)
                    : queryable.OrderBy(x => x.Name);

            if(column.Is(nameof(PhoneType.Description)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Description)
                    : queryable.OrderBy(x => x.Description);

            return queryable;
        }

        protected override IQueryable<PhoneType> SearchAllColumns(IQueryable<PhoneType> queryable, ISearch search)
        {
             return queryable.Where(x =>
                x.Name.Contains(search.Value) ||
                x.Description.Contains(search.Value));
        }

        protected override IQueryable<PhoneType> FilterColumn(IQueryable<PhoneType> queryable, IColumn column)
        {
            if(column.Is(nameof(PhoneType.Name)))
                return queryable.Where(x => x.Name.Contains(column.Search.Value));
            if(column.Is(nameof(PhoneType.Description)))
                return queryable.Where(x => x.Description.Contains(column.Search.Value));
            return queryable;
        }
    }
}
