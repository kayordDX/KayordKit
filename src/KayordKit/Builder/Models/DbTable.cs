using Humanizer;
using KayordKit.Builder.Extensions;

namespace KayordKit.Builder.Models;

public class DbTable
{

    public string DatabaseName { get; set; } = string.Empty;
    public string SchemaName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string DataModelName => TableName.RemovePluralization().Pascalize();
    public bool IsTableAttribute
    {
        get { return TableName != DataModelName; }
    }
    public List<DbTableColumn> Columns { get; set; } = new List<DbTableColumn>();
}