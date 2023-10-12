namespace KayordKit.Core.Entities;
public class DataProviderType : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
