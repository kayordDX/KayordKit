using System.Text;
using KayordKit.Builder.Extensions;
using KayordKit.Builder.Models;

namespace KayordKit.Builder.Generators;
public static class DataModelGenerator
{
    public static string GenerateDataModel(DbTable table, string dataModelNamespace)
    {
        var stringBuilder = new StringBuilder();

        var usingNamespaces = table.Columns.Select(c => c.TypeNamespace).Distinct().ToList();
        bool hasAttribute = table.Columns.Any(x => x.IsColumnAttribute == true) || table.IsTableAttribute;
        if (hasAttribute)
        {
            usingNamespaces.Add("System.ComponentModel.DataAnnotations.Schema");
        }
        usingNamespaces.Remove("System");
        usingNamespaces.Sort();
        if (usingNamespaces.Count > 0)
        {
            foreach (var usingNamespace in usingNamespaces)
            {
                stringBuilder.AppendLine($"using {usingNamespace};");
            }

            stringBuilder.AppendLine();
        }

        stringBuilder.AppendLine($"namespace {dataModelNamespace};");
        if (table.IsTableAttribute)
        {
            stringBuilder.AppendLine($"""[Table("{table.TableName}")]""");
        }
        stringBuilder.AppendLine($"public class {table.DataModelName}");
        stringBuilder.AppendLine("{");

        foreach (var column in table.Columns)
        {
            if (column.IsColumnAttribute)
            {
                stringBuilder.AppendLine($"""[Column("{column.ColumnName}")]""");
            }

            string extra = (column.Type?.GetNameForCoding() == "string") ? " = string.Empty;" : string.Empty;
            stringBuilder.AppendLine($"\tpublic {column.Type?.GetNameForCoding()} {column.DataPropertyName} {{ get; set; }}{extra}");
        }

        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine();

        return stringBuilder.ToString().TabsToSpaces();
    }
    public static string GenerateDataModelFromTable(DatabaseTable table, string dataModelNamespace = "")
    {
        var stringBuilder = new StringBuilder();

        var usingNamespaces = table.Columns.Select(c => c.TypeNamespace).Distinct().ToList();
        usingNamespaces.Remove("System");
        usingNamespaces.Sort();
        if (usingNamespaces.Count > 0)
        {
            foreach (var usingNamespace in usingNamespaces)
            {
                stringBuilder.AppendLine($"using {usingNamespace};");
            }

            stringBuilder.AppendLine();
        }

        if (string.IsNullOrEmpty(dataModelNamespace))
        {
            dataModelNamespace = $"DapperCodeGenerator.{table.DatabaseName.CapitalizeFirstLetter()}.Models";
        }

        stringBuilder.AppendLine($"namespace {dataModelNamespace};");
        stringBuilder.AppendLine($"public class {table.DataModelName}");
        stringBuilder.AppendLine("{");

        foreach (var column in table.Columns)
        {
            string extra = (column.Type?.GetNameForCoding() == "string") ? " = string.Empty;" : string.Empty;
            stringBuilder.AppendLine($"\tpublic {column.Type?.GetNameForCoding()} {column.ColumnName} {{ get; set; }}{extra}");
        }

        stringBuilder.AppendLine("}");

        stringBuilder.AppendLine();

        return stringBuilder.ToString().TabsToSpaces();
    }
}
