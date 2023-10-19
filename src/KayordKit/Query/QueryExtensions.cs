using KayordKit.Query.Models;
using QueryKit;

namespace KayordKit.Query;
public static class QueryExtensions
{

    public static IQueryable<TEntity> ApplyQuery<TEntity>(this IQueryable<TEntity> source, ApiQuery apiQuery) where TEntity : class
    {
        IQueryable<TEntity> queryable = source;

        if (!string.IsNullOrWhiteSpace(apiQuery.Filters))
        {
            var parsedFilter = QueryFilter.GetParsed(apiQuery.Filters);
            string filter = QueryFilter.ParsedToString(parsedFilter);
            queryable = queryable.ApplyQueryKitFilter(filter);
        }

        if (!string.IsNullOrWhiteSpace(apiQuery.Sorts))
        {
            var parsedSort = QuerySort.GetParsed(apiQuery.Sorts);
            string sort = QuerySort.ParsedToString(parsedSort);
            queryable = queryable.ApplyQueryKitSort(sort);
        }
        return queryable;
    }

    public static SqlKata.Query ApplyQuery(this SqlKata.Query query, ApiQuery apiQuery)
    {
        QuerySort.ApplySort(query, apiQuery.Sorts);
        QueryFilter.ApplyFilter(query, apiQuery.Filters);
        QueryPaging.Apply(query, apiQuery.Page, apiQuery.PageSize);
        return query;
    }
}
