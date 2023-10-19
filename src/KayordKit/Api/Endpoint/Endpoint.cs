using System.Reflection;
using KayordKit.Models;
using Microsoft.AspNetCore.Routing;

namespace KayordKit.Api.Endpoint;
public class Endpoint : EndpointWithoutRequest<PagedList<Response>>
{
    public override void Configure()
    {
        Post("endpoint");
        AllowAnonymous();
    }

    private readonly IEnumerable<EndpointDataSource> _endpoints;

    public Endpoint(IEnumerable<EndpointDataSource> endpoints)
    {
        _endpoints = endpoints;
    }

    public string GetTypePropertiesAsString(PropertyInfo[] properties)
    {
        string results = "";
        foreach (var prop in properties)
        {
            results += prop.Name + " - " + prop.PropertyType;
        }
        return results;
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var epDefinitions = _endpoints
            .SelectMany(s => s.Endpoints)
            .SelectMany(e => e.Metadata.OfType<EndpointDefinition>())
            .ToList();

        var result = epDefinitions.Select(d => new Response
        {
            RouteName = d.Routes?[0].ToString() ?? "",
            EndpointName = d.EndpointType.Name,
            ReqDtoName = d.ReqDtoType.Name,
            ResDtoName = d.ResDtoType.Name
        }).ToList().AsQueryable();

        var queryResponse = PagedList<Response>.Create(result, 1, 2);
        HttpContext.Response.Headers.Add("X-Pagination", queryResponse.GetPagingHeader());
        await SendAsync(queryResponse);
    }
}