using KayordKit.Builder.Models;

namespace KayordKit.Builder.SchemaInfo;

public interface ISchemaInfo
{
    public List<Models.SchemaInfo> GetTables();
    public List<Models.SchemaInfo> GetViews();
    public List<Models.SchemaInfo> GetProcedures();
    public List<Models.SchemaInfo> GetFunctions();
    public DbTable GetTableColumns(string schema, string table);
    public List<DbTableColumn> GetViewColumns(string schema, string view);
    public List<DbTableColumn> GetSQLColumns(string sql);
}