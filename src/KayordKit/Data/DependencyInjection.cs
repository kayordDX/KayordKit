using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KayordKit.Data;
public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        // services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            // options.EnableSensitiveDataLogging();
        });

        // services.AddDbContext<KayordContext>(options =>
        // {
        //     options.UseNpgsql(connectionString);
        // });

        // services.AddDbContext<ApplicationDbContext>((sp, options) =>
        // {
        //     // options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        //     options.UseSqlite(connectionString);
        // });

        // services.AddScoped(provider => provider.GetRequiredService<ApplicationDbContext>());

        // TODO: Add back init
        // services.AddScoped<AppDbContextInit>();
        services.AddSingleton<DbConnectionContext>();


        // services.AddSingleton(TimeProvider.System);
        // services.AddTransient<IIdentityService, IdentityService>();

        // services
        //     .AddIdentityCore<ApplicationUser>()
        //     // .AddRoles<IdentityRole>()
        //     .AddEntityFrameworkStores<ApplicationDbContext>()
        //     .AddApiEndpoints();

        // services.AddAuthorization(options =>
        //     options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}