using KayordKit.Enums;

namespace KayordKit.Models;

public class ProviderDTO
{
    public DbConnectionTypes Type { get; set; }
    public string ConnectionString { get; set; } = string.Empty;

}