using System.Text;
using KayordKit.Query.Models;

namespace KayordKit.Query;

public static class QuerySort
{
    private static Sort GetSortFromSortItem(string sortItem)
    {
        return new Sort
        {
            Name = sortItem.StartsWith("-") ? sortItem.Substring(1) : sortItem,
            IsDescending = sortItem.StartsWith("-")
        };
    }

    public static List<Sort> GetParsed(string? sort)
    {
        List<Sort> results = new();
        if (sort == null)
        {
            return results;
        }
        foreach (var sortItem in sort.Split(','))
        {
            if (string.IsNullOrWhiteSpace(sortItem)) continue;

            var sortTerm = GetSortFromSortItem(sortItem.Trim());

            if (results.All(s => s.Name != sortTerm.Name))
            {
                results.Add(sortTerm);
            }
        }
        return results;
    }

    public static string ParsedToString(List<Sort>? parsed)
    {
        if (parsed == null) return string.Empty;

        StringBuilder result = new();
        for (int i = 0; i < parsed.Count; i++)
        {
            if (i > 0)
            {
                result.Append(',');
            }
            if (parsed[i].IsDescending)
            {
                result.Append('-');
            }
            result.Append(parsed[i].Name);
        }
        return result.ToString();
    }

    public static SqlKata.Query ApplySort(this SqlKata.Query query, ApiQuery apiQuery)
    {
        return ApplySort(query, apiQuery.Sorts);
    }

    public static SqlKata.Query ApplySort(this SqlKata.Query query, string? sorts)
    {
        if (sorts == null)
        {
            return query;
        }

        var parsed = GetParsed(sorts);

        foreach (var sort in parsed)
        {
            if (sort.IsDescending)
            {
                query = query.OrderByDesc(sort.Name);
            }
            else
            {
                query = query.OrderBy(sort.Name);
            }
        }
        return query;
    }
}