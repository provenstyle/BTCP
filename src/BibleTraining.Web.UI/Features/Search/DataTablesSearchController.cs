namespace BibleTraining.Web.UI.Features.Search
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Results;
    using DataTables.AspNet.Core;
    using DataTables.AspNet.WebApi2;
    using FluentValidation;
    using Highway.Data;
    using Improving.Highway.Data.Scope.Repository;

    public abstract class DataTablesSearchController<TEntity, TDomain> : ApiController
        where TEntity : class
        where TDomain : class, IDomain
    {
        protected readonly IRepository<TDomain> _repository;

        protected DataTablesSearchController(IRepository<TDomain> repository)
        {
            _repository = repository;
        }

        protected abstract IQueryable<TEntity> DefaultSort(IQueryable<TEntity> queryable);
        protected abstract IQueryable<TEntity> SortColumn(IQueryable<TEntity> queryable, IColumn column);
        protected abstract IQueryable<TEntity> SearchAllColumns(IQueryable<TEntity> queryable, ISearch search);
        protected abstract IQueryable<TEntity> FilterColumn(IQueryable<TEntity> queryable, IColumn column);

        [HttpPost]
        public JsonResult<IDataTablesResponse> Search(IDataTablesRequest request)
        {
            if(request == null)
                throw new ValidationException("Invalid request");

            using (_repository.Scopes.CreateReadOnly())
            {
                var queryable = _repository.DomainContext.AsQueryable<TEntity>();
                var dataCount = queryable.Count();

                //global search
                if (!string.IsNullOrEmpty(request.Search?.Value))
                    queryable = SearchAllColumns(queryable, request.Search);

                //column filter
                var searchable = request.Columns.Where(x =>
                    x.IsSearchable &&
                    !string.IsNullOrEmpty(x.Search?.Value)).ToArray();
                foreach (var column in searchable)
                    queryable = FilterColumn(queryable, column);

                //Handle regex?

                //order
                var sortable = request.Columns.Where(x => x.IsSortable && x.Sort != null).ToArray();
                if (sortable.Any())
                {
                    foreach (var column in sortable)
                        queryable = SortColumn(queryable, column);
                }
                else
                {
                    queryable = DefaultSort(queryable);
                }

                var filteredCount = queryable.Count();

                //page
                var page = queryable.Skip(request.Start).Take(request.Length).ToArray();

                return new DataTablesJsonResult(request.CreateResponse(dataCount, filteredCount, page), Request);
            }
        }
    }

    public static class ColumnExtensions
    {
        public static bool Is(this IColumn column, string name)
        {
            return string.Equals(column?.Field, name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}