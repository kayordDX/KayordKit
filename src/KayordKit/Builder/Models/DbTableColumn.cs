using Humanizer;

namespace KayordKit.Builder.Models;
public class DbTableColumn
{
    public DbTableColumn()
    {

    }
    public DbTableColumn(SchemaColumn dbColumn, Func<SchemaColumn, Type> getType)
    {
        DatabaseName = dbColumn.DatabaseName;
        SchemaName = dbColumn.SchemaName;
        TableName = dbColumn.TableName;
        ColumnName = dbColumn.ColumnName;
        DataType = dbColumn.DataType;
        MaxLength = dbColumn.MaxLength;
        IsNullable = dbColumn.IsNullable;
        IsPrimaryKey = dbColumn.IsPrimaryKey;
        IsIdentity = dbColumn.IsIdentity;
        var type = getType(dbColumn);
        Type = type;
        TypeNamespace = type?.Namespace ?? string.Empty;
    }

    public string DatabaseName { get; set; } = string.Empty;
    public string SchemaName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
    public string DataPropertyName => ColumnName.Pascalize();
    public bool IsColumnAttribute
    {
        get { return ColumnName != DataPropertyName; }
    }
    public string DataType { get; set; } = string.Empty;
    public Type? Type { get; set; }
    public string TypeNamespace { get; set; } = string.Empty;
    public int MaxLength { get; set; }
    public bool IsNullable { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsIdentity { get; set; }
}

