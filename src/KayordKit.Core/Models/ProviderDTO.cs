using KayordKit.Core.Enums;

namespace KayordKit.Core.Models;

public class ProviderDTO
{
    public DbConnectionTypes Type { get; set; }
    public string ConnectionString { get; set; } = string.Empty;

}