using System.Text;
using System.Text.RegularExpressions;
using NJsonSchema;

namespace KayordKit.Extensions.Swagger;

public class CustomTypeNameGenerator : DefaultTypeNameGenerator
{
    public override string Generate(JsonSchema schema, string? typeNameHint, IEnumerable<string> reservedTypeNames)
    {
        if (string.IsNullOrEmpty(typeNameHint) && !string.IsNullOrEmpty(schema.DocumentPath))
        {
            typeNameHint = schema.DocumentPath.Replace("\\", "/").Split(new char[1] { '/' }).Last();
        }

        typeNameHint = (typeNameHint ?? "").Replace("[", " Of ").Replace("]", " ").Replace("<", " Of ")
            .Replace(">", " ")
            .Replace(",", " And ")
            .Replace("  ", " ");
        string[] source = typeNameHint.Split(new char[1] { ' ' });
        typeNameHint = string.Join(string.Empty, source.Select((string p) => GenerateCustom(schema, p)));
        string text = GenerateCustom(schema, typeNameHint);
        if (string.IsNullOrEmpty(text) || reservedTypeNames.Contains(text))
        {
            text = GenerateAnonymousTypeName(typeNameHint, reservedTypeNames);
        }
        var result = RemoveIllegalCharacters(text);
        return result;
    }

    protected static string GenerateCustom(JsonSchema schema, string typeNameHint)
    {
        if (string.IsNullOrEmpty(typeNameHint) && schema.HasTypeNameTitle)
        {
            typeNameHint = schema.Title ?? string.Empty;
        }

        string text = typeNameHint;
        // int num = typeNameHint?.LastIndexOf('.') ?? (-1);
        // if (num > -1)
        // {
        //     text = typeNameHint?.Substring(num + 1) ?? string.Empty;
        // }

        return ConversionUtilities.ConvertToUpperCamelCase(text ?? "Anonymous", firstCharacterMustBeAlpha: true);
    }

    private string GenerateAnonymousTypeName(string typeNameHint, IEnumerable<string> reservedTypeNames)
    {
        if (!string.IsNullOrEmpty(typeNameHint))
        {
            if (TypeNameMappings.ContainsKey(typeNameHint))
            {
                typeNameHint = TypeNameMappings[typeNameHint];
            }

            typeNameHint = typeNameHint.Split(new char[1] { '.' }).Last();
            if (!reservedTypeNames.Contains(typeNameHint) && !ReservedTypeNames.Contains(typeNameHint))
            {
                return typeNameHint;
            }

            int num = 1;
            do
            {
                num++;
            }
            while (reservedTypeNames.Contains(typeNameHint + num));
            return typeNameHint + num;
        }

        return GenerateAnonymousTypeName("Anonymous", reservedTypeNames);
    }

    private static string RemoveIllegalCharacters(string typeName)
    {
        bool flag = false;
        for (int i = 0; i < typeName.Length; i++)
        {
            char c = typeName[i];
            if (i == 0 && (!IsEnglishLetterOrUnderScore(c) || char.IsDigit(c)))
            {
                flag = true;
                break;
            }

            if (!IsEnglishLetterOrUnderScore(c) && !char.IsDigit(c))
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            return typeName;
        }

        return DoRemoveIllegalCharacters(typeName);
    }

    private static string DoRemoveIllegalCharacters(string typeName)
    {
        char c = typeName[0];
        Regex regex = new Regex("[^a-zA-Z0-9_.]");
        StringBuilder stringBuilder = new StringBuilder(typeName);
        if (!IsEnglishLetterOrUnderScore(c) || c == '_')
        {
            if (!regex.IsMatch(c.ToString()))
            {
                stringBuilder.Insert(0, "_");
            }
            else
            {
                stringBuilder[0] = '_';
            }
        }

        MatchCollection matchCollection = regex.Matches(stringBuilder.ToString());
        for (int num = matchCollection.Count - 1; num >= 0; num--)
        {
            int index = matchCollection[num].Index;
            stringBuilder[index] = '_';
        }

        Regex regex2 = new Regex("[_]{2,}");
        string text = regex2.Replace(stringBuilder.ToString(), "_");
        return text.TrimEnd(new char[1] { '_' });
    }

    private static bool IsEnglishLetterOrUnderScore(char c)
    {
        if (c == '.') return true;

        if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z'))
        {
            return c == '_';
        }

        return true;
    }
}