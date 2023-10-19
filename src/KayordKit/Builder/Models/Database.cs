using KayordKit.Enums;

namespace KayordKit.Builder.Models;

public class Database
{
    public DbConnectionTypes ConnectionType { get; set; }
    public string DatabaseName { get; set; } = string.Empty;

    public List<DatabaseTable> Tables { get; set; } = new List<DatabaseTable>();
}

