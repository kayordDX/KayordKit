using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KayordKit.Extensions.Cors;
public static class CorsExtensions
{
    private static readonly string _allowedOrigins = "KayordOrigins";
    public static void ConfigureCors(this IServiceCollection services, string[] origins)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: _allowedOrigins,
            builder =>
            {
                builder.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public static IApplicationBuilder UseCorsKayord(this IApplicationBuilder app)
    {
        app.UseCors(_allowedOrigins);
        return app;
    }
}