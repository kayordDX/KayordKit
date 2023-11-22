using FastEndpoints.Swagger;
using KayordKit.Extensions.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace KayordKit.Extensions.Api;

public static class ApiExtensions
{
    public static void ConfigureApi(this IServiceCollection services)
    {
        services.AddFastEndpoints();
        services.AddFastEndpoints()
        .SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = AppDomain.CurrentDomain.FriendlyName;
                s.Version = "v1";
                s.MarkNonNullablePropsAsRequired();
                s.OperationProcessors.Add(new CustomOperationsProcessor());
                s.SchemaNameGenerator = new CustomSchemaNameGenerator();
            };
        });
    }

    public static IApplicationBuilder UseApi(this IApplicationBuilder app)
    {
        app.UseFastEndpoints(c =>
        {
            c.Serializer.Options.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            c.Endpoints.Configurator = ep =>
            {
                ep.Options(x => x.Produces<InternalErrorResponse>(500));
            };
        });
        app.UseSwaggerGen(_ => { }, ui => ui.Path = string.Empty);
        return app;
    }
}