using System.Text.Json.Serialization;

namespace KayordKit.Models;

public class PaginationMetaData
{
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("currentPageSize")]
    public int CurrentPageSize { get; set; }

    [JsonPropertyName("currentStartIndex")]
    public int CurrentStartIndex { get; set; }

    [JsonPropertyName("currentEndIndex")]
    public int CurrentEndIndex { get; set; }

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("hasPrevious")]
    public bool HasPrevious { get; set; }

    [JsonPropertyName("hasNext")]
    public bool HasNext { get; set; }
}