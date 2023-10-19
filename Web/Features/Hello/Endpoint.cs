namespace Web.Features.Hello;
public class Endpoint : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("hello");
        AllowAnonymous();
    }

    public Endpoint()
    {
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        await SendAsync("hello");
    }
}