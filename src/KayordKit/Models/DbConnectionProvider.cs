using System.Data;
using KayordKit.Enums;
using Npgsql;
using SqlKata.Compilers;

namespace KayordKit.Models;

public class DbConnectionProvider
{
    public string ConnectionString { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DbConnectionTypes Type { get; set; }

    public IDbConnection Connection()
    {
        if (Type == DbConnectionTypes.Postgres)
        {
            return new NpgsqlConnection(ConnectionString);
        }
        // TODO: Add sql server and other
        // else if (Type == DbConnectionTypes.MsSql)
        // {
        //     return new SqlConnection(ConnectionString);
        // }
        throw new Exception("Could not find provider");
    }

    public Compiler Compiler()
    {
        if (Type == DbConnectionTypes.Postgres)
        {
            return new PostgresCompiler { };
        }
        else if (Type == DbConnectionTypes.MsSql)
        {
            return new SqlServerCompiler { };
        }
        throw new Exception("Could not create compiler");
    }
}