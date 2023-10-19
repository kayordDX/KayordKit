using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace KayordKit.Extensions.Swagger;

public class CustomOperationsProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // string result = context.OperationDescription.Operation.OperationId;
        // result = result.Replace("ApiFeatures", "");
        // result = result.Replace("Api", "");
        // result = result.Replace("Endpoint", "");
        // context.OperationDescription.Operation.OperationId = result;
        return true;
    }
}