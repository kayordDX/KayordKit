namespace KayordKit.Api.Test;
public class Endpoint : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("test");
        AllowAnonymous();
    }

    public Endpoint()
    {
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        await SendAsync("test");
    }
}