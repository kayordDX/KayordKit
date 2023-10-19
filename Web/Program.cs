global using FastEndpoints;
using KayordKit;
using FastEndpoints.Swagger;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints(o =>
{
    o.Assemblies = new[] { KayordKit.Util.GetAssembly() };
})

.SwaggerDocument();
var app = builder.Build();

app.UseFastEndpoints().UseSwaggerGen();
app.Run();
