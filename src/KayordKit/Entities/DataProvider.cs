using KayordKit.Enums;

namespace KayordKit.Entities;
public class DataProvider : AuditableEntity
{
    public Guid Id { get; set; }
    public DbConnectionTypes Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string? DBContext { get; set; }
    public string? Namespace { get; set; }
    public string? BasePath { get; set; }
}
