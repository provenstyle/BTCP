namespace BibleTraining.Web.UI.Features.Search
{
    using System;
    using System.Linq;
    using Api.Person;
    using DataTables.AspNet.Core;
    using Entities;
    using Improving.Highway.Data.Scope.Repository;

    public class PeopleController
        : DataTablesSearchController<Person, IBibleTrainingDomain>
    {
        public PeopleController(IRepository<IBibleTrainingDomain> repository)
            :base(repository)
        {
        }

        protected override IQueryable<Person> DefaultSort(IQueryable<Person> queryable)
        {
            return queryable.OrderBy(x => x.FirstName);
        }

        protected override IQueryable<Person> SortColumn(IQueryable<Person> queryable, IColumn column)
        {
            if(column.Is(nameof(Person.FirstName)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.FirstName)
                    : queryable.OrderBy(x => x.FirstName);

            if(column.Is(nameof(Person.LastName)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.LastName)
                    : queryable.OrderBy(x => x.LastName);

            if(column.Is(nameof(Person.Gender)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.Gender)
                    : queryable.OrderBy(x => x.Gender);

            if(column.Is(nameof(Person.BirthDate)))
                return column.Sort.Direction == SortDirection.Descending
                    ? queryable.OrderByDescending(x => x.BirthDate)
                    : queryable.OrderBy(x => x.BirthDate);

            return queryable;
        }

        protected override IQueryable<Person> SearchAllColumns(IQueryable<Person> queryable, ISearch search)
        {
             return queryable.Where(x =>
                x.FirstName.Contains(search.Value) ||
                x.LastName.Contains(search.Value));
        }

        protected override IQueryable<Person> FilterColumn(IQueryable<Person> queryable, IColumn column)
        {
            if(column.Is(nameof(Person.FirstName)))
                return queryable.Where(x => x.FirstName.Contains(column.Search.Value));
            if(column.Is(nameof(Person.LastName)))
                return queryable.Where(x => x.LastName.Contains(column.Search.Value));
            if (column.Is(nameof(Person.Gender)))
            {
                Gender gender;
                if(Enum.TryParse(column.Search.Value, true, out gender))
                {
                    return queryable.Where(x => x.Gender == gender);
                }
            }
            if(column.Is(nameof(Person.BirthDate)))
                return queryable.Where(x => x.BirthDate == DateTime.Parse(column.Search.Value));

            return queryable;
        }
    }
}
