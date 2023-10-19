using KayordKit.Builder.Extensions;
using KayordKit.Enums;

namespace KayordKit.Builder.Models;
public class DatabaseTable
{
    public DbConnectionTypes ConnectionType { get; set; }
    public string DatabaseName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;

    public string DataModelName => $"{TableName.CapitalizeFirstLetter().RemovePluralization()}DTO";

    public List<DatabaseTableColumn> Columns { get; set; } = new List<DatabaseTableColumn>();
}
