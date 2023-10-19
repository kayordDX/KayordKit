using KayordKit.Builder.Models;

namespace KayordKit.Builder.Providers;

public interface IProvider
{
    public Database SelectDatabase(List<Database> databases, string databaseName);

    public List<Database> RefreshDatabases();

    public string ConnectionString { get; }
}