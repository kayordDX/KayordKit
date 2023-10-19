namespace KayordKit.Builder.Templates;

public class GenericTemplate
{
    private readonly string _ns;
    public GenericTemplate(string ns)
    {
        _ns = ns;
    }

    public string GenericRequest()
    {
        return
        $$"""
        namespace {{_ns}};
        
        public class Request
        {

        }
        """;
    }

    public string GenericResponse()
    {
        return
        $$"""
        namespace {{_ns}};

        public class Response
        {

        }
        """;
    }

    public string GenericEndpoint()
    {
        return
        $$"""
        namespace {{_ns}};

        public class Endpoint : Endpoint<Request, Response>
        {
            public override void Configure()
            {
                Post("test");
                AllowAnonymous();
            }

            public Endpoint()
            {

            }

            public override async Task HandleAsync(Request r, CancellationToken c)
            {
                await SendAsync(new Response { });
            }
        }
        """;
    }
}