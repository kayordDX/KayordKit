using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace KayordKit.Extensions.Swagger;

public class CustomOperationsProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        string result = context.OperationDescription.Operation.OperationId;
        string name = AppDomain.CurrentDomain.FriendlyName.Replace(".", string.Empty);
        string featureName = name + "Features";
        result = result.Replace(featureName, string.Empty);
        result = result.Replace("KayordKitFeatures", string.Empty);
        result = result.Replace(name, string.Empty);
        result = result.Replace("KayordKit", string.Empty);
        result = result.Replace("Endpoint", string.Empty);
        context.OperationDescription.Operation.OperationId = result;
        return true;
    }
}