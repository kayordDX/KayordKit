using Namotion.Reflection;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace KayordKit.Extensions.Swagger;

public sealed class AddAdditionalTypeProcessor<T> : IDocumentProcessor where T : class
{
    public void Process(DocumentProcessorContext context)
    {
        context.SchemaGenerator.Generate(typeof(T).ToContextualType(), context.SchemaResolver);
    }
}