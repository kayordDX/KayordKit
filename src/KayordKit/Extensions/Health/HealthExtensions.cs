using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KayordKit.Extensions.Health;

public static class HealthExtensions
{
    public static void ConfigureHealth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddProcessAllocatedMemoryHealthCheck(150)
            .AddNpgSql(connectionString: configuration.GetConnectionString("DefaultConnection")!);
    }
}