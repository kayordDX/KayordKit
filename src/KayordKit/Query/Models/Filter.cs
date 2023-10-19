namespace KayordKit.Query.Models;

public class Filter
{
    public string Operator { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}