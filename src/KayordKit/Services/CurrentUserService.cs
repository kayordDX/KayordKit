using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace KayordKit.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Sub => _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub") ?? "";
    public string? Expires => _httpContextAccessor.HttpContext?.User?.FindFirstValue("exp");
}
