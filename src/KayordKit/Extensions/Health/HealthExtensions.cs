using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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


    public static IApplicationBuilder UseHealth(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }
}