namespace KayordKit.Query.Models;

public class ApiQuery
{
    public string? Filters { get; set; }
    public string? Sorts { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}