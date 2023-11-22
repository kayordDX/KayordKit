using System.Data;
using KayordKit.Enums;
using KayordKit.Models;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace KayordKit.Data;

public class DbConnectionContext
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Dictionary<string, ProviderDTO>? _providerList;

    public DbConnectionContext(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public IDbConnection CreateConnection(string providerName)
    {
        if (_providerList == null)
        {
            RefreshProviders();
        }

        if (_providerList != null)
        {
            _providerList.TryGetValue(providerName, out var provider);
            if (provider != null)
            {
                if (provider.Type == DbConnectionTypes.Postgres)
                {
                    return new NpgsqlConnection(provider.ConnectionString);
                }
                // TODO: Add sql server and other
                // else if (provider.Type == DbConnectionTypes.MsSql)
                // {
                //     return new SqlConnection(provider.ConnectionString);
                // }
            }
        }
        throw new Exception("Could not find provider");
    }

    public DbConnectionProvider Get(string providerName)
    {
        if (_providerList == null)
        {
            RefreshProviders();
        }

        if (_providerList != null)
        {
            _providerList.TryGetValue(providerName, out var provider);
            if (provider != null)
            {
                return new Models.DbConnectionProvider
                {
                    Name = providerName,
                    ConnectionString = provider.ConnectionString ?? "",
                    Type = provider.Type,
                };
            }

        }
        throw new Exception("Could not find provider");
    }

    public void RefreshProviders()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<KayordKitDbContext>();

            _providerList = db.DataProvider.ToDictionary(x => x.Name, x =>
                new ProviderDTO
                {
                    ConnectionString = GetConnectionString(x.ConnectionString, x.Username, x.Password),
                    Type = x.Type
                }
            );
        }
    }

    private static string GetConnectionString(string connectionString, string userName, string password)
    {
        return connectionString
            .Replace("{Username}", Util.Decrypt(userName))
            .Replace("{Password}", Util.Decrypt(password));
    }
}