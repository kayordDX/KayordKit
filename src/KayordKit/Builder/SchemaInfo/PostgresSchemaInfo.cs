using System.Data;
using Dapper;
using KayordKit.Builder.Models;
using KayordKit.Models;
using NpgsqlTypes;

namespace KayordKit.Builder.SchemaInfo;

public class PostgresSchemaInfo : ISchemaInfo
{
    private readonly DbConnectionProvider _dbConnection;
    public PostgresSchemaInfo(DbConnectionProvider dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public List<Models.SchemaInfo> GetTables()
    {
        try
        {
            using var db = _dbConnection.Connection();

            const string query = @"
            select 
                table_schema Schema,
                table_name Name
            from information_schema.tables
            WHERE table_type = 'BASE TABLE'
            AND table_schema <> 'information_schema'
            AND table_schema <> 'pg_catalog'
            ";

            var results = db.Query<Models.SchemaInfo>(query);
            return results.ToList();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message, ex);
            throw new Exception("could not get tables");
        }
    }

    public List<Models.SchemaInfo> GetViews()
    {
        try
        {
            using var db = _dbConnection.Connection();

            const string query = @"
            SELECT 
                table_name Name,
                table_schema Schema
            FROM information_schema.views
            WHERE table_schema <> 'information_schema'
            AND table_schema <> 'pg_catalog'
            ";

            var results = db.Query<Models.SchemaInfo>(query);
            return results.ToList();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message, ex);
            throw new Exception("could not get views");
        }
    }

    public List<Models.DbTableColumn> GetSQLColumns(string sql)
    {
        try
        {
            using var db = _dbConnection.Connection();
            var res = db.ExecuteReader(sql);
            var schema = res.GetSchemaTable();
            if (schema?.Rows == null)
            {
                throw new Exception("no rows found");
            }
            List<DbTableColumn> results = new();
            foreach (DataRow r in schema.Rows)
            {
                DbTableColumn d = new()
                {
                    ColumnName = r["ColumnName"]?.ToString() ?? string.Empty,
                    DatabaseName = r["BaseCatalogName"]?.ToString() ?? string.Empty,
                    DataType = r["DataTypeName"]?.ToString() ?? string.Empty,
                    IsIdentity = (bool)r["IsIdentity"],
                    IsPrimaryKey = (bool)r["IsKey"],
                    // IsNullable = (bool)r["IsNullable"],
                    Type = (Type)r["DataType"],
                    TypeNamespace = ((Type)r["DataType"]).Namespace ?? string.Empty,
                    MaxLength = (int)r["ColumnSize"],
                    SchemaName = string.Empty,
                    TableName = string.Empty,
                };
                results.Add(d);
            }
            return results;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message, ex);
            throw new Exception("could not get views");
        }
    }

    public List<Models.SchemaInfo> GetProcedures()
    {
        try
        {
            using var db = _dbConnection.Connection();

            const string query = @"
            SELECT
                routine_schema Schema,
                routine_name Name
            FROM 
                information_schema.routines
            WHERE 
                routine_type = 'PROCEDURE'
                AND routine_schema <> 'information_schema'
                AND routine_schema <> 'pg_catalog'  
            ";

            var results = db.Query<Models.SchemaInfo>(query);
            return results.ToList();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message, ex);
            throw new Exception("could not get views");
        }
    }

    public List<Models.SchemaInfo> GetFunctions()
    {
        try
        {
            using var db = _dbConnection.Connection();

            const string query = @"
            SELECT
                routine_schema Schema,
                routine_name Name
            FROM 
                information_schema.routines
            WHERE 
                routine_type = 'FUNCTION'
                AND routine_schema <> 'information_schema'
                AND routine_schema <> 'pg_catalog'  
            ";

            var results = db.Query<Models.SchemaInfo>(query);
            return results.ToList();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message, ex);
            throw new Exception("could not get views");
        }
    }

    public DbTable GetTableColumns(string schema, string table)
    {
        try
        {
            using var db = _dbConnection.Connection();

            const string columnsQuery = @"
            SELECT 
                c.table_catalog DatabaseName,
                c.table_schema SchemaName,
                c.table_name TableName,
                c.column_name ColumnName,
                c.udt_name DataType,
                CAST(COALESCE(c.character_maximum_length, 0) AS INTEGER) MaxLength,
                CAST(c.is_nullable AS bool) IsNullable,
                CAST(c.is_identity AS bool) IsIdentity,
                case when pk.column_name is null then false else true end as IsPrimaryKey
            FROM INFORMATION_SCHEMA.COLUMNS AS c
            LEFT JOIN (
              SELECT 
                c.column_name
              FROM information_schema.table_constraints tc 
              JOIN information_schema.constraint_column_usage AS ccu USING (constraint_schema, constraint_name) 
              JOIN information_schema.columns AS c ON c.table_schema = tc.constraint_schema
                AND tc.table_name = c.table_name AND ccu.column_name = c.column_name
              WHERE tc.constraint_type = 'PRIMARY KEY' 
              AND tc.table_name = @table
            ) pk
            ON c.column_name = pk.column_name
            WHERE TABLE_NAME = @table
            AND table_schema = @schema
            ";

            var pgColumns = db.Query<SchemaColumn>(columnsQuery, new { schema, table });

            DbTable result = new()
            {
                TableName = table,
                SchemaName = schema,
                Columns = pgColumns.Select(c => new DbTableColumn(c, GetType)).ToList(),
            };
            return result;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message, ex);
            throw new Exception("could not get tableColumn");
        }
    }

    public List<DbTableColumn> GetViewColumns(string schema, string view)
    {
        try
        {
            using var db = _dbConnection.Connection();

            const string query = @"
            select 
                table_catalog DatabaseName,
                table_schema SchemaName,
                table_name TableName,
                column_name ColumnName,
                udt_name DataType,
                CAST(COALESCE(character_maximum_length, 0) AS INTEGER) MaxLength,
                CAST(is_nullable AS bool) IsNullable,
                CAST(is_identity AS bool) IsIdentity,
                false IsPrimaryKey
            from information_schema.columns
            join pg_class on table_name = relname and relnamespace=table_schema::regnamespace
            where relkind = 'v'
            AND table_schema = @schema
            AND table_name = @view
            ";

            var results = db.Query<SchemaColumn>(query, new { schema, view });
            return results.Select(c => new DbTableColumn(c, GetType)).ToList();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message, ex);
            throw new Exception("could not get views");
        }
    }

    protected static Type GetType(SchemaColumn dbColumn)
    {
        switch (dbColumn.DataType)
        {
            case "int8":
                return dbColumn.IsNullable ? typeof(long?) : typeof(long);

            case "bytea":
                return typeof(byte[]);

            case "bool":
            case "boolean":
                return dbColumn.IsNullable ? typeof(bool?) : typeof(bool);

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
                return dbColumn.IsNullable ? typeof(DateTime?) : typeof(DateTime);

            case "numeric":
            case "money":
                return dbColumn.IsNullable ? typeof(decimal?) : typeof(decimal);

            case "float8":
                return dbColumn.IsNullable ? typeof(double?) : typeof(double);

            case "int4":
            case "integer":
                return dbColumn.IsNullable ? typeof(int?) : typeof(int);

            case "float4":
                return dbColumn.IsNullable ? typeof(float?) : typeof(float);

            case "uuid":
                return dbColumn.IsNullable ? typeof(Guid?) : typeof(Guid);

            case "int2":
                return dbColumn.IsNullable ? typeof(short?) : typeof(short);

            case "tinyint":
                return dbColumn.IsNullable ? typeof(byte?) : typeof(byte);

            case "structured":
                return typeof(DataTable);

            case "timetz":
                return dbColumn.IsNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);

            default:
                return typeof(object);
        }
    }
}