namespace KayordKit.Core.Entities;
public class User : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public bool PhoneNumberConfirmed { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool TwoFactorEnabled { get; set; }
    public int LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
}
