using KayordKit.Builder.Providers;
using KayordKit.Enums;
using KayordKit.Models;

namespace KayordKit.Builder;

public static class BuilderFactory
{
    public static IProvider GetBuilder(DbConnectionProvider dapperProvider)
    {
        IProvider? provider = null;
        if (dapperProvider.Type == DbConnectionTypes.Postgres)
        {
            provider = new PostgresProvider(dapperProvider.ConnectionString);
        }
        else if (dapperProvider.Type == DbConnectionTypes.MsSql)
        {
            provider = new MsSqlProvider(dapperProvider.ConnectionString);
        }

        if (provider == null)
        {
            throw new Exception("not implemented");
        }
        return provider;
    }
}