namespace KayordKit.Builder.Models;

public class SchemaColumn
{
    public string DatabaseName { get; set; } = string.Empty;
    public string SchemaName { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public int MaxLength { get; set; }
    public bool IsNullable { get; set; }
    public bool IsIdentity { get; set; }
    public bool IsPrimaryKey { get; set; }
}