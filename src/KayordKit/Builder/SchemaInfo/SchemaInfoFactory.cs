using KayordKit.Enums;
using KayordKit.Models;

namespace KayordKit.Builder.SchemaInfo;

public static class SchemaInfoFactory
{
    public static ISchemaInfo GetBuilder(DbConnectionProvider dapperProvider)
    {
        ISchemaInfo? builder = null;
        if (dapperProvider.Type == DbConnectionTypes.Postgres)
        {
            builder = new PostgresSchemaInfo(dapperProvider);
        }
        else if (dapperProvider.Type == DbConnectionTypes.MsSql)
        {
            builder = null;
        }

        if (builder == null)
        {
            throw new Exception("not implemented");
        }
        return builder;
    }
}