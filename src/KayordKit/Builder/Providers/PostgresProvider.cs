using System.Data;
using Dapper;
using KayordKit.Builder.Models;
using KayordKit.Enums;
using Microsoft.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;

namespace KayordKit.Builder.Providers;
public class PostgresProvider : Provider
{
    private readonly string[] _systemDatabases = { "system", "postgres", "template0", "template1" };
    private readonly string[] _systemTables = { "VersionInfo", "pg_", "sql_" };

    private readonly NpgsqlConnectionStringBuilder _connectionStringBuilder;

    public PostgresProvider(string connectionString)
        : base(connectionString)
    {
        _connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
        _connectionStringBuilder.Database = "";
    }

    protected override IEnumerable<Database> GetDatabases()
    {
        DataTable? databases = null;
        try
        {
            using var db = new NpgsqlConnection(_connectionStringBuilder.ToString());
            db.Open();
            databases = db.GetSchema(SqlClientMetaDataCollectionNames.Databases);
            db.Close();
        }
        catch (Exception exc)
        {
            Console.Error.WriteLine(exc.Message, exc);
        }

        if (databases != null)
        {
            foreach (DataRow databaseRow in databases.Rows)
            {
                var databaseName = databaseRow?.ItemArray[0]?.ToString();
                if (!_systemDatabases.Any(d => d.Equals(databaseName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var database = new Database
                    {
                        ConnectionType = DbConnectionTypes.Postgres,
                        DatabaseName = databaseName ?? ""
                    };
                    yield return database;
                }
            }
        }
    }

    protected override IEnumerable<DatabaseTable> GetDatabaseTables(string databaseName)
    {
        DataTable? selectedDatabaseTables = null;
        try
        {
            using var db = new NpgsqlConnection($"{_connectionStringBuilder};Database={databaseName};");
            db.Open();
            selectedDatabaseTables = db.GetSchema(SqlClientMetaDataCollectionNames.Tables);
            db.Close();
        }
        catch (Exception exc)
        {
            Console.Error.WriteLine(exc.Message, exc);
        }

        if (selectedDatabaseTables != null)
        {
            foreach (DataRow tableRow in selectedDatabaseTables.Rows)
            {
                var tableName = tableRow?.ItemArray[2]?.ToString();
                if (!_systemTables.Any(t => (tableName ?? "").StartsWith(t)))
                {
                    var table = new DatabaseTable
                    {
                        ConnectionType = DbConnectionTypes.Postgres,
                        DatabaseName = databaseName,
                        TableName = tableName ?? ""
                    };
                    yield return table;
                }
            }
        }
    }

    protected override IEnumerable<DatabaseTableColumn> GetDatabaseTableColumns(string databaseName, string tableName)
    {
        DataTable? selectedDatabaseTableColumns = null;
        DataTable? selectedDatabaseTablePrimaryColumns = null;
        try
        {
            using var db = new NpgsqlConnection($"{_connectionStringBuilder};Database={databaseName};");
            db.Open();
            var columnRestrictions = new string[3];
            columnRestrictions[0] = databaseName;
            columnRestrictions[2] = tableName;

            selectedDatabaseTableColumns = db.GetSchema(SqlClientMetaDataCollectionNames.Columns, columnRestrictions);

            const string columnsQuery = @"
            SELECT 
                column_name ColumnName,
                data_type DataType,
                CAST(COALESCE(character_maximum_length, 0) AS INTEGER) DataLength,
                CAST(is_nullable AS bool) IsNullable
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = @TableName
            ";

            var pgColumns = db.Query<SchemaColumn>(columnsQuery, new { tableName });

            selectedDatabaseTablePrimaryColumns = db.GetSchema(SqlClientMetaDataCollectionNames.IndexColumns, columnRestrictions);
            db.Close();
        }
        catch (Exception exc)
        {
            Console.Error.WriteLine(exc.Message, exc);
        }

        if (selectedDatabaseTableColumns != null)
        {
            foreach (DataRow columnRow in selectedDatabaseTableColumns.Rows)
            {
                var columnName = columnRow?.ItemArray[3]?.ToString();
                var isNullable = columnRow?.ItemArray[6]?.ToString() == "YES";
                var dataType = columnRow?.ItemArray[7]?.ToString();
                var type = GetClrType(dataType ?? "", isNullable);
                var maxLengthStr = columnRow?.ItemArray[8]?.ToString();
                int.TryParse(maxLengthStr, out var maxLength);

                var column = new DatabaseTableColumn
                {
                    ConnectionType = DbConnectionTypes.Postgres,
                    DatabaseName = databaseName,
                    TableName = tableName,
                    ColumnName = columnName ?? "",
                    DataType = dataType ?? "",
                    Type = type,
                    TypeNamespace = type.Namespace ?? "",
                    MaxLength = maxLength
                };

                if (selectedDatabaseTablePrimaryColumns != null)
                {
                    foreach (DataRow indexColumnRow in selectedDatabaseTablePrimaryColumns.Rows)
                    {
                        // var indexId = indexColumnRow[3].ToString();
                        // var indexColumnName = indexColumnRow[4].ToString();
                        var indexId = indexColumnRow[7].ToString(); // 7 or 2
                        var indexColumnName = indexColumnRow[6].ToString();

                        if (indexColumnName == columnName)
                        {
                            if ((indexId ?? "").StartsWith("pk_", StringComparison.InvariantCultureIgnoreCase) ||
                                (indexId ?? "").EndsWith("_pk", StringComparison.InvariantCultureIgnoreCase) ||
                                (indexId ?? "").EndsWith("_pkey", StringComparison.InvariantCultureIgnoreCase))
                            {
                                column.PrimaryKeys.Add(indexId ?? "");
                            }
                        }
                    }
                }

                yield return column;
            }
        }
    }

    protected override Type GetClrType(string dbTypeName, bool isNullable)
    {
        switch (dbTypeName)
        {
            case "int8":
                return isNullable ? typeof(long?) : typeof(long);

            case "bytea":
                return typeof(byte[]);

            case "bool":
            case "boolean":
                return isNullable ? typeof(bool?) : typeof(bool);

            case "text":
            case "varchar":
            case "character varying":
            case "bpchar":
            case "citext":
            case "json":
            case "jsonb":
            case "xml":
            case "name":
                return typeof(string);

            case "point":
                return typeof(NpgsqlPoint);

            case "lseg":
                return typeof(NpgsqlLSeg);

            case "path":
                return typeof(NpgsqlPath);

            case "polygon":
                return typeof(NpgsqlPolygon);

            case "line":
                return typeof(NpgsqlLine);

            case "circle":
                return typeof(NpgsqlCircle);

            case "box":
                return typeof(NpgsqlBox);

            case "date":
            case "timestamp":
            case "timestamp without time zone":
            case "timestamp with time zone":
            case "timestamptz":
                return isNullable ? typeof(DateTime?) : typeof(DateTime);

            case "numeric":
            case "money":
                return isNullable ? typeof(decimal?) : typeof(decimal);

            case "float8":
                return isNullable ? typeof(double?) : typeof(double);

            case "int4":
            case "integer":
                return isNullable ? typeof(int?) : typeof(int);

            case "float4":
                return isNullable ? typeof(float?) : typeof(float);

            case "uuid":
                return isNullable ? typeof(Guid?) : typeof(Guid);

            case "int2":
                return isNullable ? typeof(short?) : typeof(short);

            case "tinyint":
                return isNullable ? typeof(byte?) : typeof(byte);

            case "structured":
                return typeof(DataTable);

            case "timetz":
                return isNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);

            default:
                return typeof(object);
        }
    }
}
