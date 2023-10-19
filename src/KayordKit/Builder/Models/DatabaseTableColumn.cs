using KayordKit.Enums;

namespace KayordKit.Builder.Models;
public class DatabaseTableColumn
{
    public DbConnectionTypes ConnectionType { get; set; }
    public string DatabaseName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;

    public string DataType { get; set; } = string.Empty;
    public Type? Type { get; set; }
    public string TypeNamespace { get; set; } = string.Empty;
    public int MaxLength { get; set; }

    public List<string> PrimaryKeys { get; set; } = new List<string>();
    public bool IsPrimaryKey => PrimaryKeys?.Count > 0;

    public List<string> IdentityKeys { get; set; } = new List<string>();
    public bool IsIdentity => IdentityKeys?.Count > 0;
}

