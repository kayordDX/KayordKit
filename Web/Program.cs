global using FastEndpoints;
using KayordKit.Extensions.Host;
using KayordKit.Extensions.Api;
using KayordKit.Extensions.Health;


var builder = WebApplication.CreateBuilder(args);
builder.Host.AddLoggingConfiguration(builder.Configuration);

builder.Services.ConfigureApi();

builder.Services.AddHealthChecks()
            .AddProcessAllocatedMemoryHealthCheck(150);

var app = builder.Build();

app.UseApi();
app.UseHealth();

app.Run();
