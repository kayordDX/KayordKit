namespace KayordKit.Services;

public interface ICurrentUserService
{
    string Sub { get; }
    // IEnumerable<Claim>? Roles { get; }
    string? Expires { get; }
}
