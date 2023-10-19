using System.Text;
using System.Text.RegularExpressions;
using KayordKit.Query.Enums;
using KayordKit.Query.Models;

namespace KayordKit.Query;

public static class QueryFilter
{
    private const string OperatorsRegEx = @"(!@=\*|!_=\*|!_-=\*|!=\*|!@=|!_=|!_-=|==\*|@=\*|_=\*|_-=\*|==|!=|>=|<=|>|<|@=|_=|_-=)";

    public static List<Filter> GetParsed(string? filter)
    {
        List<Filter> results = new();
        if (filter == null)
        {
            return results;
        }

        var filters = filter.Split(',');
        foreach (var f in filters)
        {
            if (string.IsNullOrWhiteSpace(f)) continue;
            var filterSplits = Regex.Split(f, OperatorsRegEx).Select(t => t.Trim()).ToArray();
            if (filterSplits.Length > 2)
            {
                results.Add(new Filter
                {
                    Name = filterSplits[0],
                    Operator = filterSplits[1],
                    Value = filterSplits[2]
                });
            }
        }
        return results;
    }

    private static FilterOperator GetOperator(string o)
    {
        switch (o.TrimEnd('*'))
        {
            case "==":
                return FilterOperator.Equals;
            case "!=":
                return FilterOperator.NotEquals;
            case "<":
                return FilterOperator.LessThan;
            case ">":
                return FilterOperator.GreaterThan;
            case ">=":
                return FilterOperator.GreaterThanOrEqualTo;
            case "<=":
                return FilterOperator.LessThanOrEqualTo;
            case "@=":
            case "!@=":
                return FilterOperator.Contains;
            case "_=":
            case "!_=":
                return FilterOperator.StartsWith;
            case "_-=":
            case "!_-=":
                return FilterOperator.EndsWith;
            default:
                return FilterOperator.Equals;
        }
    }

    public static string ParsedToString(List<Filter>? parsed)
    {
        if (parsed == null) return string.Empty;

        StringBuilder result = new();
        for (int i = 0; i < parsed.Count; i++)
        {
            if (i > 0)
            {
                // TODO: Need to check if same name and change this to or
                result.Append(" && ");
            }
            result.Append(parsed[i].Name);
            result.Append(parsed[i].Operator);
            result.Append('"');
            result.Append(parsed[i].Value);
            result.Append('"');
        }
        return result.ToString();
    }

    public static SqlKata.Query ApplyFilter(this SqlKata.Query query, ApiQuery apiQuery)
    {
        return ApplyFilter(query, apiQuery.Filters);
    }

    public static SqlKata.Query ApplyFilter(this SqlKata.Query query, string? filters)
    {
        if (filters == null)
        {
            return query;
        }

        var parsed = GetParsed(filters);
        // var duplicates = parsed.GroupBy(x => x.Name)
        //     .Where(g => g.Count() > 1)
        //     .Select(x => x.Key)
        //     .ToList();

        // Dictionary<string, int> duplicateDictionary = new();
        // foreach (var dup in duplicates)
        // {
        //     duplicateDictionary.Add(dup, 0);
        // }

        foreach (var filter in parsed)
        {
            FilterOperator filterOperator = GetOperator(filter.Operator);
            bool isCaseSensitive = !filter.Operator.EndsWith('*');
            // TODO: Need to check if same name and change this to OrWhere

            if (filterOperator == FilterOperator.Equals)
            {
                query = query.Where(filter.Name, filter.Value);
            }
            else if (filterOperator == FilterOperator.NotEquals)
            {
                query = query.WhereNot(filter.Name, filter.Value);
            }
            else if (filterOperator == FilterOperator.GreaterThan
                || filterOperator == FilterOperator.LessThan
                || filterOperator == FilterOperator.GreaterThanOrEqualTo
                || filterOperator == FilterOperator.LessThanOrEqualTo
            )
            {
                query = query.Where(filter.Name, filter.Operator, filter.Value);
            }
            else if (filterOperator == FilterOperator.Contains)
            {
                query = query.WhereContains(filter.Name, filter.Value, isCaseSensitive);
            }
            else if (filterOperator == FilterOperator.StartsWith)
            {
                query = query.WhereStarts(filter.Name, filter.Value, isCaseSensitive);
            }
            else if (filterOperator == FilterOperator.EndsWith)
            {
                query = query.WhereEnds(filter.Name, filter.Value, isCaseSensitive);
            }
            else if (filterOperator == FilterOperator.NotContains)
            {
                query = query.WhereNotContains(filter.Name, filter.Value, isCaseSensitive);
            }
            else if (filterOperator == FilterOperator.NotStartsWith)
            {
                query = query.WhereNotStarts(filter.Name, filter.Value, isCaseSensitive);
            }
            else if (filterOperator == FilterOperator.NotEndsWith)
            {
                query = query.WhereNotEnds(filter.Name, filter.Value, isCaseSensitive);
            }
        }
        return query;
    }
}