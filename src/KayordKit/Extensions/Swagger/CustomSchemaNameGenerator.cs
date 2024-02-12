using System.Text;
using NJsonSchema.Generation;

namespace KayordKit.Extensions.Swagger;

internal class CustomSchemaNameGenerator : DefaultSchemaNameGenerator, ISchemaNameGenerator
{
    readonly bool _shortSchemaNames = false;

    public CustomSchemaNameGenerator(bool shortSchemaNames)
    {
        _shortSchemaNames = shortSchemaNames;
    }

    public override string Generate(Type type)
    {
        var isGeneric = type.IsGenericType;
        var fullNameWithoutGenericArgs =
            isGeneric
                ? type.FullName![..type.FullName!.IndexOf('`')]
                : type.FullName;

        if (_shortSchemaNames)
        {
            var index = fullNameWithoutGenericArgs!.LastIndexOf('.');
            index = index == -1 ? 0 : index + 1;
            var shortName = fullNameWithoutGenericArgs[index..];

            return isGeneric
                       ? shortName + GenericArgString(type)
                       : shortName;
        }
        var sanitizedFullName = fullNameWithoutGenericArgs!.Replace(".", string.Empty);
        string name = AppDomain.CurrentDomain.FriendlyName.Replace(".", string.Empty);
        string featureName = name + "Features";
        sanitizedFullName = sanitizedFullName.Replace(featureName, string.Empty);
        sanitizedFullName = sanitizedFullName.Replace(name, string.Empty);
        sanitizedFullName = sanitizedFullName.Replace("FastEndpoints", string.Empty);
        sanitizedFullName = sanitizedFullName.Replace("Endpoint", string.Empty);

        return isGeneric
                   ? sanitizedFullName + GenericArgString(type)
                   : sanitizedFullName;

        static string? GenericArgString(Type type)
        {
            if (type.IsGenericType)
            {
                var sb = new StringBuilder();
                var args = type.GetGenericArguments();

                for (var i = 0; i < args.Length; i++)
                {
                    var arg = args[i];
                    if (i == 0)
                        sb.Append("Of");
                    sb.Append(TypeNameWithoutGenericArgs(arg));
                    sb.Append(GenericArgString(arg));
                    if (i < args.Length - 1)
                        sb.Append("And");
                }

                return sb.ToString();
            }

            return type.Name;

            static string TypeNameWithoutGenericArgs(Type type)
            {
                var index = type.Name.IndexOf('`');
                index = index == -1 ? 0 : index;

                return type.Name[..index];
            }
        }
    }
}