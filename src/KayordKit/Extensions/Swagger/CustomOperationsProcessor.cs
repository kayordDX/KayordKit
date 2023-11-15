using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace KayordKit.Extensions.Swagger;

public class CustomOperationsProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        string result = context.OperationDescription.Operation.OperationId;
        string name = AppDomain.CurrentDomain.FriendlyName.Replace(".", "");
        string featureName = name + "Features";
        result = result.Replace(featureName, "");
        result = result.Replace("KayordKitFeatures", "");
        result = result.Replace(name, "");
        result = result.Replace("KayordKit", "");
        result = result.Replace("Endpoint", "");
        context.OperationDescription.Operation.OperationId = result;
        return true;
    }
}