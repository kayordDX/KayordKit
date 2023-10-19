using Namotion.Reflection;
using NJsonSchema.Annotations;
using NJsonSchema.Generation;

namespace KayordKit.Extensions.Swagger;

internal class CustomSchemaNameGenerator : DefaultSchemaNameGenerator, ISchemaNameGenerator
{
    public override string Generate(Type type)
    {
        CachedType cachedType = type.ToCachedType();
        JsonSchemaAttribute? inheritedAttribute = cachedType.GetInheritedAttribute<JsonSchemaAttribute>();
        if (!string.IsNullOrEmpty(inheritedAttribute?.Name))
        {
            return inheritedAttribute.Name;
        }

        CachedType cachedType2 = type.ToCachedType();
        if (cachedType2.Type.IsConstructedGenericType)
        {
            return GetName(cachedType2).Split(new char[1] { '`' }).First() + "Of" + string.Join("And", cachedType2.GenericArguments.Select((CachedType a) => Generate(a.OriginalType)));
        }

        return GetName(cachedType2);
    }

    private static string GetName(CachedType cType)
    {
        if (cType.TypeName == "Int16")
        {
            return GetNullableDisplayName(cType, "Short");
        }
        else if (cType.TypeName == "Int32")
        {
            return GetNullableDisplayName(cType, "Integer");
        }
        else if (cType.TypeName == "Int64")
        {
            return GetNullableDisplayName(cType, "Long");
        }
        string name = cType.Type?.FullName ?? cType.TypeName ?? string.Empty;
        return GetNullableDisplayName(cType, ReplaceName(name));
    }

    private static string ReplaceName(string name)
    {
        name = name.Replace("FastEndpoints.", "");
        // result = result.Replace("Kayord.Api.Features", "");
        // result = result.Replace(".", "");
        // result = result.Replace("FastEndpoints", "");
        // // result = result.Replace("Kayord", "");
        // // result = result.Replace("SieveModels", "");
        // // result = result.Replace("CommonEnums", "");
        // // result = result.Replace("CommonModels", "");
        return name;
    }

    private static string GetNullableDisplayName(CachedType type, string actual)
    {
        return (type.IsNullableType ? "Nullable" : "") + actual;
    }
}