using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Text;

namespace DbReflector
{

    public enum FunctionName
    {
        ProcParameters,
        Procs,
        TableColumns,
        UdtColumns,
        TablesAndViews,
        Udts,
    }

    public class SqlHelper
    {
        SQLiteConnection _SqliteConnection;
        SQLiteCommand _SqliteCommand;
        SQLiteDataAdapter _SqliteAdapter;

        SqlConnection _connection;
        SqlCommand _cmd;
        SqlDataAdapter _da;

        string _applicationName;
        string _connectionString;

        public static readonly string SqliteDb ="URI=file:" + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),  "DbReflector.db"); //@"URI=file:C:\trash\dbReflector.db"
        SqlHelper(string connectionString, string applicationName, bool refreshMetaData)
        {
            _connectionString = connectionString;
            _applicationName = applicationName;
            
            _SqliteConnection = new SQLiteConnection(SqliteDb);
            _SqliteConnection.Open();
            _SqliteCommand = new SQLiteCommand();
            _SqliteCommand.Connection = _SqliteConnection;
            _SqliteAdapter = new SQLiteDataAdapter(_SqliteCommand);


            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            _cmd = new SqlCommand();
            _cmd.Connection = _connection;
            _da = new SqlDataAdapter(_cmd);

            if (refreshMetaData)
            {
                //create the configuration tables in an Sqlite database
                ExecuteSqliteNonQuery(TransformSql(Tables.CreateTable_TableColumns_2));
                ExecuteSqliteNonQuery(TransformSql(Tables.CreateTable_TablesAndViews_4));
                ExecuteSqliteNonQuery(TransformSql(Procs.CreateTable_Procs_7));
                ExecuteSqliteNonQuery(TransformSql(Procs.CreateTable_ProcParams_9));
                ExecuteSqliteNonQuery(TransformSql(Procs.CreateTable_ProcColumns_10_1));
                ExecuteSqliteNonQuery(TransformSql(Udts.CreateTable_Udts_11));
                ExecuteSqliteNonQuery(TransformSql(Udts.CreateTable_UdtColumns_13));

                InsertIntoTableColumns(ExecuteDataTable(TransformSql(Tables.RefreshTable_TableColumns_3)));
                InsertIntoTablesAndViews(ExecuteDataTable(TransformSql(Tables.RefreshTable_TablesAndViews_5)));
                UpdateTablesAndViewsPrimaryKeysColumn(ExecuteSqliteDataTable(TransformSql(Tables.AddPrimaryKeysToTablesAndViews_6)));
                UpdateTablesAndViewsForeignKeysColumn(ExecuteSqliteDataTable(TransformSql(Tables.AddForeignKeysToTablesAndViews_6_1)));
                InsertIntoProcs(ExecuteDataTable(TransformSql(Procs.RefreshTable_Procs_8)));
                InsertIntoProcParams(ExecuteDataTable(TransformSql(Procs.RefreshTable_ProcParams_10)));
                InsertIntoProcColumns(ExecuteDataTable(TransformSql(Procs.RefreshTable_ProcColumns_10_2)));
                InsertIntoUdts(ExecuteDataTable(TransformSql(Udts.RefreshTable_Udts_12)));
                InsertIntoUdtColumns(ExecuteDataTable(TransformSql(Udts.RefreshTable_UdtColumns_14)));
            }
        }

        private void UpdateTablesAndViewsPrimaryKeysColumn(DataTable table)
        {
            var sb = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                sb.Append($"update TablesAndViews set primary_keys='{row["Primary_Keys"]}' where application_name='{row["application_name"]}' and table_schema='{row["TABLE_SCHEMA"]}' and table_name ='{row["TABLE_NAME"]}';\n");
            }
            ExecuteSqliteNonQuery(sb.ToString());
        }
        private void UpdateTablesAndViewsForeignKeysColumn(DataTable table)
        {
            var sb = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                sb.Append($"update TablesAndViews set foreign_keys='{row["foreign_keys"]}' where application_name='{row["application_name"]}' and TABLE_SCHEMA='{row["TABLE_SCHEMA"]}' and table_name ='{row["TABLE_NAME"]}';\n");
            }
            ExecuteSqliteNonQuery(sb.ToString());
        }

        public static SqlHelper CreateInstance(string connectionstring, string applicationName, bool refreshMetaData = false) => new SqlHelper(connectionstring, applicationName, refreshMetaData);

        public DataTable GetDataTable(string sql, string name)
        {
            DataTable table = new DataTable(name);
            _SqliteCommand.CommandText = sql;
            _SqliteCommand.CommandType = CommandType.Text;
            SQLiteDataAdapter da = new SQLiteDataAdapter(_SqliteCommand);

            da.Fill(table);
            return table;
        }
        void ExecuteSqliteNonQuery(string sql)
        {
            try
            {
                _SqliteCommand.CommandType = CommandType.Text;
                _SqliteCommand.CommandText = sql;
                _SqliteCommand.CommandTimeout = 60;
                _SqliteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        DataTable ExecuteSqliteDataTable(string sql)
        {
            try
            {
                _SqliteCommand.CommandType = CommandType.Text;
                _SqliteCommand.CommandText = sql;
                _SqliteCommand.CommandTimeout = 60;
                DataTable table = new DataTable();
                _SqliteAdapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        DataTable ExecuteDataTable(string sql)
        {
            try
            {
                _cmd.CommandType = CommandType.Text;
                _cmd.CommandText = sql;
                _cmd.CommandTimeout = 60;
                DataTable table = new DataTable();
                _da.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        void InsertIntoUdtColumns(DataTable table)
        {
            if (table.Rows.Count == 0) return;
            var sql = $"INSERT INTO UdtColumns(application_name,schema_name,type_name,column_name,data_type,ordinal_number,max_length,precision,is_nullable,is_identity,is_computed) VALUES";

            var values = new List<string>();

            for (int row_index = 0; row_index < table.Rows.Count; row_index++)
            {
                DataRow row = table.Rows[row_index];
                values.Add($"('{row["application_name"]}','{row["schema_name"]}','{row["type_name"]}','{row["column_name"]}','{row["data_type"]}','{row["ordinal_number"]}','{row["max_length"]}','{row["precision"]}','{row["is_nullable"]}','{row["is_identity"]}','{row["is_computed"]}')");
            }
            sql += string.Join(",", values);
            _SqliteCommand.CommandText = sql;
            _SqliteCommand.ExecuteNonQuery();
        }
        void InsertIntoProcColumns(DataTable table)
        {
            var sql = $"INSERT INTO ProcColumns(APPLICATION_NAME,SCHEMA_NAME,PROC_NAME,ORDINAL_POSITION,COLUMN_NAME,IsNullable,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,DOMAIN_NAME) VALUES";

            var values = new List<string>();

            for (int row_index = 0; row_index < table.Rows.Count; row_index++)
            {
                DataRow row = table.Rows[row_index];
                values.Add($"('{row["application_name"]}','{row["schema_name"]}','{row["proc_name"]}','{row["ordinal_position"]}','{row["column_name"]}','{row["is_nullable"]}','{row["data_type"]}','{row["character_maximum_length"]}','{row["domain_name"]}')");
            }
            sql += string.Join(",", values);
            _SqliteCommand.CommandText = sql;
            _SqliteCommand.ExecuteNonQuery();
        }

        void InsertIntoProcParams(DataTable table)
        {
            var sql = $"INSERT INTO ProcParams (application_name,SchemaName,ProcName,ParameterName,OrdinalNumber,Direction,IsResult,ParameterType,ParameterMaxLen,ParameterNumericPrecision,ParameterDatePrecision,ParameterUDTName) VALUES";

            var values = new List<string>();

            for (int row_index = 0; row_index < table.Rows.Count; row_index++)
            {
                DataRow row = table.Rows[row_index];
                values.Add($"('{row["application_name"]}','{row["SPECIFIC_SCHEMA"]}','{row["ROUTINE_NAME"]}','{row["PARAMETER_NAME"]}','{row["ORDINAL_POSITION"]}','{row["PARAMETER_MODE"]}','{row["IS_RESULT"]}','{row["DATA_TYPE"]}','{row["CHARACTER_MAXIMUM_LENGTH"]}','{row["NUMERIC_PRECISION"]}','{row["DATETIME_PRECISION"]}','{row["USER_DEFINED_TYPE_NAME"]}')");
            }
            sql += string.Join(",", values);
            _SqliteCommand.CommandText = sql;
            _SqliteCommand.ExecuteNonQuery();
        }
        void InsertIntoProcs(DataTable table)
        {
            var sql = $"INSERT INTO Procs (application_name,SchemaName,ProcName,IgnoreProc,SourceCode) VALUES";

            var values = new List<string>();

            for (int row_index = 0; row_index < table.Rows.Count; row_index++)
            {
                DataRow row = table.Rows[row_index];
                values.Add($"('{row["application_name"]}','{row["SchemaName"]}','{row["ProcName"]}','{row["IgnoreProc"]}','{row["SourceCode"].ToString().Replace("'","''")}')");
            }
            sql += string.Join(",", values);
            _SqliteCommand.CommandText = sql;
            _SqliteCommand.ExecuteNonQuery();
        }
        void InsertIntoUdts(DataTable table)
        {
            var sql = $"INSERT INTO Udts(application_name,schema_name,udt_name,ignore_udt) VALUES";

            var values = new List<string>();

            for (int row_index = 0; row_index < table.Rows.Count; row_index++)
            {
                DataRow row = table.Rows[row_index];
                values.Add($"('{row["application_name"]}','{row["schema_name"]}','{row["udt_name"]}','{row["ignore_udt"]}')");
            }
            if (values.Count == 0) return;
            sql += string.Join(",", values);
            _SqliteCommand.CommandText = sql;
            _SqliteCommand.ExecuteNonQuery();
        }
        void InsertIntoTableColumns(DataTable table)
        {
            var sql = $"INSERT INTO TableColumns (application_name,TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME,IsPrimaryKey,IsForeignKey,IsKey,IsNullable,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,ORDINAL_POSITION,NUMERIC_PRECISION,DOMAIN_NAME,IGNORE_COLUMN,COLUMN_ALIAS) VALUES";

            var values = new List<string>();

            for (int row_index = 0; row_index < table.Rows.Count; row_index++)
            {
                DataRow row = table.Rows[row_index];
                values.Add($"('{row["application_name"]}','{row["TABLE_SCHEMA"]}','{row["TABLE_NAME"]}','{row["COLUMN_NAME"]}','{row["IsPrimaryKey"]}','{row["IsForeignKey"]}','{row["IsKey"]}','{row["IsNullable"]}','{row["DATA_TYPE"]}','{row["CHARACTER_MAXIMUM_LENGTH"]}','{row["ORDINAL_POSITION"]}','{row["NUMERIC_PRECISION"]}','{row["DOMAIN_NAME"]}','{row["IGNORE_COLUMN"]}','{row["COLUMN_ALIAS"]}')");
            }
            sql += string.Join(",", values);
            _SqliteCommand.CommandText = sql;
            _SqliteCommand.ExecuteNonQuery();
        }

        void InsertIntoTablesAndViews(DataTable table)
        {
            var sql = $"INSERT INTO TablesAndViews(application_name,table_schema,table_name,table_type,table_alias,IgnoreTable,is_enum) VALUES";

            var values = new List<string>();

            for (int row_index = 0; row_index < table.Rows.Count; row_index++)
            {
                DataRow row = table.Rows[row_index];
                values.Add($"('{row["application_name"]}','{row["table_schema"]}','{row["table_name"]}','{row["table_type"]}','{row["table_alias"]}','{row["IgnoreTable"]}','{row["is_enum"]}')");
            }
            sql += string.Join(",", values);
            _SqliteCommand.CommandText = sql;
            _SqliteCommand.ExecuteNonQuery();
        }

        #region Code Templates
        const string CreateSchema_DbReflector_1 = @"
IF not exists(SELECT * FROM sys.schemas WHERE name = N'DbReflector')
BEGIN	
	declare @sql nvarchar(4000) = 'create schema DbReflector';
    EXEC sp_executesql @sql
End
";

        public string TransformSql(string s) => s.Replace("<@applicationName@>", _applicationName).Replace("<@excludedSchemasList@>", "'DbReflector','bTbParams','sys'");

        static class Tables
        {
            public const string CreateTable_TableColumns_2 = @"
drop table if exists TableColumns;create table TableColumns
(
    application_name nvarchar(50) not null
    ,TABLE_SCHEMA nvarchar(50) not null
    ,TABLE_NAME nvarchar(50) not null
    ,COLUMN_NAME nvarchar(50) not null
    ,IsPrimaryKey bit not null
    ,IsForeignKey bit  not null
    ,IsKey bit not null
    ,IsNullable bit not null
    ,DATA_TYPE nvarchar(50) not null
    ,CHARACTER_MAXIMUM_LENGTH int
    ,ORDINAL_POSITION int not null
    ,NUMERIC_PRECISION tinyint
    ,DOMAIN_NAME nvarchar(50)
    ,IGNORE_COLUMN bit not null default(0)
    ,COLUMN_ALIAS nvarchar(50) not null
)
;create unique index u_bTb_tblcols on TableColumns (application_name, TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME)
;create unique index u_bTb_tblcols_colalias on TableColumns (application_name, TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME)";
            public const string RefreshTable_TableColumns_3 = @"
declare @app varchar(50) = '<@applicationName@>';
SET NOCOUNT ON
SET XACT_ABORT ON;

with cte as( 
SELECT 
c.TABLE_SCHEMA ,C.TABLE_NAME ,c.COLUMN_NAME, TC.CONSTRAINT_TYPE ,cast(iif(tc.CONSTRAINT_TYPE='PRIMARY KEY',1,0) as INT) IsPrimaryKey ,cast(iif(tc.CONSTRAINT_TYPE='FOREIGN KEY',1,0) as INT) IsForeignKey 
,cast(iif(tc.CONSTRAINT_TYPE IS null,0,1) as INT) IsKey ,cast(iif(c.is_nullable='NO',0,1) as bit) IsNullable ,c.DATA_TYPE ,c.CHARACTER_MAXIMUM_LENGTH ,c.NUMERIC_PRECISION ,C.DOMAIN_NAME ,C.ORDINAL_POSITION 
FROM information_schema.COLUMNS c 
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE u ON c.table_schema=u.table_schema AND c.table_name=u.table_name AND c.column_name=u.column_name 
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc ON tc.CONSTRAINT_NAME=U.CONSTRAINT_NAME 
where 
	c.table_name <> 'database_firewall_rules' 
	and c.table_name <> 'sysdiagrams' 
	and c.table_name <> '__MigrationHistory' 
	and c.column_name not like 'graph_id_%' 
	and c.column_name not like '$edge_id_%' 
	and c.column_name not like 'to_obj_id_%' 
	and c.column_name not like 'from_obj_id_%' 
	and c.column_name not like '$to_obj_id_%' 
	and c.column_name not like '$to_id_%' 
	and c.column_name not like 'to_id_%' 
	and c.column_name not like '$node_id_%' 
	and c.column_name not like '$from_id_%'
	and c.table_schema not in (<@excludedSchemasList@>) 
),
	source as (SELECT @app as application_name, c.TABLE_SCHEMA, c.TABLE_NAME, c.COLUMN_NAME, c.COLUMN_NAME COLUMN_ALIAS, cast(MAX(c.IsPrimaryKey) as bit) IsPrimaryKey, cast(MAX(c.IsForeignKey) as bit)IsForeignKey, cast(MAX(c.IsKey) as bit) IsKey, c.IsNullable, c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH, c.NUMERIC_PRECISION , c.DOMAIN_NAME,c.ORDINAL_POSITION, cast(0 as bit) IGNORE_COLUMN FROM cte c GROUP BY c.TABLE_SCHEMA, c.TABLE_NAME, c.COLUMN_NAME, c.IsNullable, c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH, c.NUMERIC_PRECISION , c.DOMAIN_NAME, c.ORDINAL_POSITION )
select * from source
";
            public const string CreateTable_TablesAndViews_4 = @"
drop table if exists TablesAndViews;create table TablesAndViews
(
    application_name nvarchar(50) not null
	,table_schema nvarchar(50) not null
	,table_name nvarchar(50)   not null
	,table_type nvarchar(50)   not null
	,table_alias nvarchar(50)  not null
	,IgnoreTable bit           not null
    ,is_enum bit not null 
	,primary_keys nvarchar(2000)        
	,foreign_keys nvarchar(2000)        
)
;CREATE UNIQUE INDEX u_tblvws ON  TablesAndViews (application_name , table_schema,table_name)
;CREATE UNIQUE INDEX u_tblvws_alias ON  TablesAndViews (application_name ,table_schema,table_alias);
";
            public const string RefreshTable_TablesAndViews_5 = @"
declare @app varchar(50) = '<@applicationName@>'
SET NOCOUNT ON
SET XACT_ABORT ON;
SELECT @app application_name, table_schema,table_name,table_type,table_name table_alias, 0 IgnoreTable,0 is_enum,''primary_keys,''foreign_keys FROM INFORMATION_SCHEMA.tables where table_schema not in (<@excludedSchemasList@>)
";
            public const string AddPrimaryKeysToTablesAndViews_6 = @"
SELECT tc.application_name, tc.TABLE_SCHEMA, tc.TABLE_NAME, group_concat(tc.COLUMN_NAME) Primary_Keys FROM TableColumns tc
         WHERE tc.IsPrimaryKey = 'True'
         AND tc.application_name='<@applicationName@>'
         GROUP BY tc.application_name,tc.TABLE_SCHEMA, tc.TABLE_NAME
";
            public const string AddForeignKeysToTablesAndViews_6_1 = @"
SELECT tc.application_name, tc.TABLE_SCHEMA, tc.TABLE_NAME, group_concat(tc.COLUMN_NAME) foreign_keys FROM TableColumns tc
         WHERE tc.IsForeignKey = 'True'
         AND tc.application_name='<@applicationName@>'
         GROUP BY tc.application_name,tc.TABLE_SCHEMA, tc.TABLE_NAME
";
        }

        static class Procs
        {

            public const string CreateTable_Procs_7 = @"
drop table if exists Procs;create table Procs
(
    application_name nvarchar(50) not null
	,SchemaName nvarchar(50) not null
	,ProcName nvarchar(150) not null
	,IgnoreProc bit not null
    ,SourceCode text not null
)
;create unique index bTb_procs on Procs(application_name,SchemaName,ProcName)
";
            public const string RefreshTable_Procs_8 = @"
declare @app varchar(50) = '<@applicationName@>';
SET NOCOUNT ON;
SET XACT_ABORT ON;
WITH source
     AS (SELECT @app application_name, 
                r.SPECIFIC_SCHEMA SchemaName, 
                r.ROUTINE_NAME ProcName, 
                CAST(0 AS BIT) IgnoreProc,
                r.ROUTINE_DEFINITION SourceCode
         FROM information_schema.routines AS r
         WHERE routine_type = 'PROCEDURE'
               AND LEFT(Routine_Name, 3) NOT IN('sp_', 'xp_', 'ms_')
         AND r.ROUTINE_BODY = 'SQL'
         AND r.SPECIFIC_SCHEMA NOT IN(<@excludedSchemasList@>))
select * from source
";
            public const string CreateTable_ProcParams_9 = @"
drop table if exists ProcParams;create table ProcParams
( 
        application_name nvarchar(50) not null,
		SchemaName NVARCHAR(50) NOT NULL, 
		ProcName NVARCHAR(150) NOT NULL, 
		ParameterName NVARCHAR(150) NOT NULL, 
		OrdinalNumber INT NOT NULL, 
		Direction VARCHAR(10) NOT NULL, 
		IsResult BIT NOT NULL, 
		ParameterType NVARCHAR(50) NOT NULL, 
		ParameterMaxLen INT NULL, 
		ParameterNumericPrecision INT NULL, 
		ParameterDatePrecision INT NULL, 
		ParameterUDTName NVARCHAR(50) NULL
)
;create unique index bTb_procparam on ProcParams(application_name, SchemaName,ProcName,ParameterName)
";
            public const string RefreshTable_ProcParams_10 = @"
DECLARE @app VARCHAR(50)= '<@applicationName@>';
SET NOCOUNT ON
SET XACT_ABORT ON;
WITH source AS 
(
	SELECT @app application_name,
        r.SPECIFIC_SCHEMA, 
       r.ROUTINE_NAME, 
       p.PARAMETER_NAME, 
       p.ORDINAL_POSITION, 
       p.PARAMETER_MODE, 
       cast(IIF(p.IS_RESULT='NO',0,1) as bit) IS_RESULT, 
       p.DATA_TYPE, 
       p.CHARACTER_MAXIMUM_LENGTH, 
       p.NUMERIC_PRECISION, 
       p.DATETIME_PRECISION, 
       p.USER_DEFINED_TYPE_NAME 
FROM information_schema.routines AS r
     INNER JOIN information_schema.parameters AS p ON r.specific_schema = p.specific_schema
                                                      AND r.specific_name = p.specific_name
WHERE routine_type = 'PROCEDURE'
    AND LEFT(Routine_Name, 3) NOT IN('sp_', 'xp_', 'ms_')
    AND r.ROUTINE_BODY = 'SQL'
    AND r.SPECIFIC_SCHEMA not in (<@excludedSchemasList@>)
)
select * from source
";
            public const string CreateTable_ProcColumns_10_1 = @"
drop table if exists ProcColumns;create table ProcColumns
(
    APPLICATION_NAME nvarchar(50) not null,
	SCHEMA_NAME nvarchar(50) not null,
	PROC_NAME nvarchar(150) not null,
	ORDINAL_POSITION int not null,
	COLUMN_NAME nvarchar(150) not null,
	IsNullable bit not null,
	DATA_TYPE  nvarchar(50) not null,
	CHARACTER_MAXIMUM_LENGTH int,
    DOMAIN_NAME nvarchar(50),
    PRIMARY KEY (APPLICATION_NAME, SCHEMA_NAME, PROC_NAME, COLUMN_NAME)
)
";
            public const string RefreshTable_ProcColumns_10_2 = @"
declare @app varchar(50) = '<@applicationName@>'
SET NOCOUNT ON;
SET XACT_ABORT ON;
WITH source
     AS (SELECT @app application_name, s.name schema_name, 
                p.name proc_name, 
                column_ordinal ordinal_position, 
                isnull(r.name, 'id') column_name, 
                is_nullable, 
                system_type_name data_type, 
                max_length character_maximum_length, 
                user_type_name domain_name
         FROM sys.procedures AS p
              CROSS APPLY sys.dm_exec_describe_first_result_set_for_object(p.object_id, 0) AS r
              INNER JOIN sys.schemas AS s ON s.schema_id = p.schema_id
         WHERE is_hidden = 0 AND s.name NOT IN (<@excludedSchemasList@>))
select * from source
";
        }

        static class Udts
        {
            public const string CreateTable_Udts_11 = @"
drop table if exists Udts;create table Udts
(
    application_name nvarchar(50) not null,
	schema_name nvarchar(50) not null,
	udt_name nvarchar(150) not null,
	ignore_udt bit not null
)
;create unique index bTb_udts on udts(application_name, schema_name,udt_name)
";
            public const string RefreshTable_Udts_12 = @"
declare @app varchar(50) = '<@applicationName@>'
SET NOCOUNT ON
SET XACT_ABORT ON;

with source as
(
	select @app application_name, s.name schema_name, t.name udt_name,0 ignore_udt from sys.types t inner join sys.schemas s on t.schema_id=s.schema_id
	where is_user_defined = 1 and is_table_type=1 and s.name not in (<@excludedSchemasList@>)
)
select * from source
";
            public const string CreateTable_UdtColumns_13 = @"
drop table if exists UdtColumns;create table UdtColumns
(
    application_name nvarchar(50) not null,
    schema_name nvarchar(50) not null,
    type_name NVARCHAR(150) NOT NULL, 
    column_name NVARCHAR(150) NOT NULL, 
    data_type NVARCHAR(50) NOT NULL, 
    ordinal_number INT NOT NULL, 
    max_length INT , 
    precision int,
    is_nullable bit not null,
    is_identity bit not null, 
    is_computed bit not null
)
;create unique index bTb_udtcols on UdtColumns (application_name,schema_name,type_name,column_name)
";
            public const string RefreshTable_UdtColumns_14 = @"
declare @app varchar(50) = '<@applicationName@>'
SET NOCOUNT ON
SET XACT_ABORT ON;
with source as (
select 
	@app application_name
	,ss.name schema_name
	,t.name [type_name]
	,c.name [column_name]
	,y.name [data_type]
	,c.column_id ordinal_number
	,c.max_length
	,c.precision
	,c.is_nullable
	,c.is_identity
	,c.is_computed
from sys.table_types t
inner join sys.columns c on c.object_id = t.type_table_object_id
inner join sys.types y on y.user_type_id = c.user_type_id
inner join sys.schemas ss on ss.schema_id = t.schema_id
where t.is_user_defined = 1
  and t.is_table_type = 1
  and ss.name not in (<@excludedSchemasList@>)
  ) 
select * from source
";
        }
        #endregion
    }
}