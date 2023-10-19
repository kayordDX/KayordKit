namespace KayordKit.Query;

public static class QueryPaging
{
    public static SqlKata.Query Apply(this SqlKata.Query query, int? page, int? pageSize)
    {
        if (page == null && pageSize == null)
        {
            return query;
        }

        int pageNumber = page ?? 0;
        int pSize = pageSize ?? 0;
        query = query.Skip((pageNumber - 1) * pSize).Take(pSize);
        return query;
    }
}