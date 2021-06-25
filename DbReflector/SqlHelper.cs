using Microsoft.Data.SqlClient;
using System;
using System.Data;

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
        SqlConnection _cn;
        SqlCommand _cmd;
        string _connectionString;
        string _applicationName;

        SqlHelper(string connectionString, string applicationName, bool refreshMetaData)
        {
            _connectionString = connectionString;
            _applicationName = applicationName;
            _cn = new SqlConnection(connectionString);
            _cn.Open();
            _cmd = new SqlCommand();
            _cmd.Connection = _cn;

            if (refreshMetaData)
            {

                ExecuteNonQuery(CreateSchema_DbReflector_1); //create the DbReflector schema if it does not exist.

                ExecuteNonQuery(TransformSql(Tables.CreateTable_TableColumns_2)); //create the configuration table that stores all the tables and views
                ExecuteNonQuery(TransformSql(Tables.RefreshTable_TableColumns_3));
                ExecuteNonQuery(TransformSql(Tables.CreateTable_TablesAndViews_4));
                ExecuteNonQuery(TransformSql(Tables.RefreshTable_TablesAndViews_5));
                ExecuteNonQuery(TransformSql(Tables.AddPrimaryKeysToTablesAndViews_6));
                ExecuteNonQuery(TransformSql(Tables.AddForeignKeysToTablesAndViews_6_1));

                ExecuteNonQuery(TransformSql(Procs.CreateTable_Procs_7));
                ExecuteNonQuery(TransformSql(Procs.RefreshTable_Procs_8));
                ExecuteNonQuery(TransformSql(Procs.CreateTable_ProcParams_9));
                ExecuteNonQuery(TransformSql(Procs.RefreshTable_ProcParams_10));
                ExecuteNonQuery(TransformSql(Procs.CreateTable_ProcColumns_10_1));
                ExecuteNonQuery(TransformSql(Procs.RefreshTable_ProcColumns_10_2));

                ExecuteNonQuery(TransformSql(Udts.CreateTable_Udts_11));
                ExecuteNonQuery(TransformSql(Udts.RefreshTable_Udts_12));
                ExecuteNonQuery(TransformSql(Udts.CreateTable_UdtColumns_13));
                ExecuteNonQuery(TransformSql(Udts.RefreshTable_UdtColumns_14));
            }
        }
        public static SqlHelper CreateInstance(string connectionstring, string applicationName, bool refreshMetaData = false) => new SqlHelper(connectionstring, applicationName, refreshMetaData);

        public DataTable GetDataTable(string sql, string name)
        {
            DataTable table = new DataTable(name);
            _cmd.CommandText = sql;
            _cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(_cmd);
            da.Fill(table);
            return table;
        }
        void ExecuteNonQuery(string sql)
        {
            try
            {
                _cmd.CommandType = CommandType.Text;
                _cmd.CommandText = sql;
                _cmd.CommandTimeout = 60;
                _cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
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
IF NOT exists(SELECT * FROM information_schema.tables WHERE table_schema=N'DbReflector' and table_name = 'TableColumns')
BEGIN
	CREATE TABLE DbReflector.TableColumns
	(
        application_name nvarchar(50) not null,
		TABLE_SCHEMA nvarchar(50) not null,
		TABLE_NAME nvarchar(50) not null,
		COLUMN_NAME nvarchar(50) not null,
		IsPrimaryKey bit not null,
		IsForeignKey bit  not null,
		IsKey bit not null,
		IsNullable bit not null,
		DATA_TYPE  nvarchar(50) not null,
		CHARACTER_MAXIMUM_LENGTH int,
		ORDINAL_POSITION int not null,
		NUMERIC_PRECISION tinyint,
        DOMAIN_NAME nvarchar(50),
		IGNORE_COLUMN bit not null default(0),
		COLUMN_ALIAS nvarchar(50) not null
	)
	create unique index u_bTb_tblcols on DbReflector.tablecolumns (application_name, table_schema,table_name,column_name)
	create unique index u_bTb_tblcols_colalias on DbReflector.tablecolumns (application_name, table_schema,table_name,column_alias)
END
";

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
	source as (SELECT @app as application_name, c.TABLE_SCHEMA, c.TABLE_NAME, c.COLUMN_NAME, c.COLUMN_NAME COLUMN_ALIAS, cast(MAX(c.IsPrimaryKey) as bit) IsPrimaryKey, cast(MAX(c.IsForeignKey) as bit)IsForeignKey, cast(MAX(c.IsKey) as bit) IsKey, c.IsNullable, c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH, c.NUMERIC_PRECISION , c.DOMAIN_NAME,c.ORDINAL_POSITION, cast(0 as bit) IGNORE_COLUMN FROM cte c GROUP BY c.TABLE_SCHEMA, c.TABLE_NAME, c.COLUMN_NAME, c.IsNullable, c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH, c.NUMERIC_PRECISION , c.DOMAIN_NAME, c.ORDINAL_POSITION ),
	dest as (SELECT application_name, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, COLUMN_ALIAS, IsPrimaryKey, IsForeignKey, IsKey, IsNullable, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION , DOMAIN_NAME,ORDINAL_POSITION, IGNORE_COLUMN from DbReflector.TableColumns)
	
	MERGE dest
	USING source
	ON source.application_name = dest.application_name and source.table_schema = dest.table_schema and source.table_name=dest.table_name AND source.column_name=dest.column_name AND source.table_schema not in (<@excludedSchemasList@>)
	WHEN MATCHED THEN
		UPDATE SET
			dest.IsPrimaryKey=source.IsPrimaryKey,
			dest.IsForeignKey=source.IsForeignKey,
			dest.IsKey=source.IsKey,
			dest.IsNullable=source.IsNullable,
			dest.Data_type=source.data_type,
			dest.character_maximum_length=source.character_maximum_length,
			dest.NUMERIC_PRECISION=source.NUMERIC_PRECISION,
			dest.domain_name=source.domain_name,
			dest.ordinal_position=source.ordinal_position
	WHEN NOT MATCHED BY TARGET THEN
		INSERT 
		(application_name, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, COLUMN_ALIAS, IsPrimaryKey, IsForeignKey, IsKey, IsNullable, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION , DOMAIN_NAME,ORDINAL_POSITION, IGNORE_COLUMN)
		VALUES
		(source.application_name, source.TABLE_SCHEMA, source.TABLE_NAME, source.COLUMN_NAME, source.COLUMN_ALIAS, source.IsPrimaryKey, source.IsForeignKey, source.IsKey, source.IsNullable, source.DATA_TYPE, source.CHARACTER_MAXIMUM_LENGTH, source.NUMERIC_PRECISION , source.DOMAIN_NAME,source.ORDINAL_POSITION, source.IGNORE_COLUMN)
;
delete dbreflector.TableColumns where application_name =@app and table_schema + '.' + table_name + '.' +  column_name not in (
	SELECT table_schema + '.' + table_name +'.'+column_name FROM INFORMATION_SCHEMA.COLUMNS where table_schema not in (<@excludedSchemasList@>))
";
            public const string CreateTable_TablesAndViews_4 = @"
IF NOT exists(SELECT * FROM information_schema.tables WHERE table_schema=N'DbReflector' and table_name = 'TablesAndViews')
BEGIN
	CREATE TABLE DbReflector.TablesAndViews
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
    CREATE UNIQUE INDEX u_tblvws ON  DbReflector.TablesAndViews (application_name , table_schema,table_name);
	CREATE UNIQUE INDEX u_tblvws_alias ON  DbReflector.TablesAndViews (application_name ,table_schema,table_alias);
END
";
            public const string RefreshTable_TablesAndViews_5 = @"
--working
declare @app varchar(50) = '<@applicationName@>'
SET NOCOUNT ON
SET XACT_ABORT ON;

WITH source AS
(
	SELECT @app application_name, table_schema,table_name,table_type,0 IgnoreTable,0 is_enum FROM INFORMATION_SCHEMA.tables where table_schema not in (<@excludedSchemasList@>) 
)
,

dest AS (SELECT application_name, table_schema,table_name,table_type,table_alias,IgnoreTable, is_enum FROM DbReflector.TablesAndViews)

MERGE	dest
USING	source
ON source.application_name=dest.application_name and source.table_schema = dest.table_schema and source.table_name=dest.table_name
WHEN NOT MATCHED BY TARGET	THEN	
	INSERT	(application_name, table_schema,table_name,table_type,table_alias,IgnoreTable, is_enum)
	VALUES
	(source.application_name, source.table_schema,source.table_name,source.table_type,source.table_name,source.IgnoreTable, source.is_enum)
;

delete dbreflector.tablesandviews where application_name =@app and table_schema + '.' + table_name not in (
	SELECT table_schema + '.' + table_name FROM INFORMATION_SCHEMA.tables where table_schema not in (<@excludedSchemasList@>))

";
            public const string AddPrimaryKeysToTablesAndViews_6 = @"
with dest as
(select application_name, table_schema, table_name, primary_keys from dbReflector.TablesAndViews where application_name = '<@applicationName@>')
, source as
(SELECT tc.application_name, tc.TABLE_SCHEMA, tc.TABLE_NAME, STRING_AGG(tc.COLUMN_NAME, ' ,') Primary_Keys FROM DbReflector.TableColumns tc
         WHERE tc.IsPrimaryKey = 1
         AND tc.application_name='<@applicationName@>'
         GROUP BY tc.application_name,tc.TABLE_SCHEMA, tc.TABLE_NAME)

merge dest
using source
ON dest.table_schema = source.TABLE_SCHEMA
AND dest.table_name = source.table_name
AND dest.application_name = '<@applicationName@>'
when matched then
	update set dest.primary_keys=source.primary_keys
;
";
            public const string AddForeignKeysToTablesAndViews_6_1 = @"
with dest as
(select application_name, table_schema, table_name, foreign_keys from dbReflector.TablesAndViews where application_name = '<@applicationName@>')
, source as
(SELECT tc.application_name, tc.TABLE_SCHEMA, tc.TABLE_NAME, STRING_AGG(tc.COLUMN_NAME, ' ,') foreign_keys FROM DbReflector.TableColumns tc
         WHERE tc.IsForeignKey = 1
         AND tc.application_name='<@applicationName@>'
         GROUP BY tc.application_name,tc.TABLE_SCHEMA, tc.TABLE_NAME)
merge dest
using source
ON dest.table_schema = source.TABLE_SCHEMA
AND dest.table_name = source.table_name
AND dest.application_name = '<@applicationName@>'
when matched then
update set dest.foreign_keys=source.foreign_keys;
";
        }

        static class Procs
        {

            public const string CreateTable_Procs_7 = @"
IF NOT exists(SELECT * FROM information_schema.tables WHERE table_schema=N'DbReflector' and table_name = 'Procs')
BEGIN
	CREATE TABLE DbReflector.Procs
	(
        application_name nvarchar(50) not null
		,SchemaName nvarchar(50) not null
		,ProcName nvarchar(150) not null
		,IgnoreProc bit not null
        ,SourceCode nvarchar(max) not null
	)
	create unique index bTb_procs on DbReflector.procs(application_name,schemaname,procname)
END
";
            public const string RefreshTable_Procs_8= @"
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
         AND r.SPECIFIC_SCHEMA NOT IN(<@excludedSchemasList@>)),
     dest
     AS (SELECT application_name, 
                schemaname, 
                procname, 
                ignoreproc,
                SourceCode
         FROM DbReflector.procs)
     MERGE dest
     USING source
     ON source.application_name = dest.application_name
        AND dest.schemaname = source.schemaname
        AND dest.procname = source.procname
		WHEN MATCHED THEN
			UPDATE SET dest.SourceCode = source.SourceCode
         WHEN NOT MATCHED BY TARGET
         THEN
           INSERT(application_name, 
                  SchemaName, 
                  ProcName, 
                  IgnoreProc,
				  SourceCode)
           VALUES
     (source.application_name, 
      source.SchemaName, 
      source.ProcName, 
      source.IgnoreProc,
	  source.SourceCode
     );
DELETE dbreflector.procs
WHERE application_name = @app
      AND SchemaName+'.'+ProcName NOT IN
(
    SELECT routine_schema+'.'+specific_name
    FROM INFORMATION_SCHEMA.routines
    WHERE routine_schema NOT IN(<@excludedSchemasList@>)
);
";
            public const string CreateTable_ProcParams_9 = @"
IF NOT exists(SELECT * FROM information_schema.tables WHERE table_schema=N'DbReflector' and table_name = 'ProcParams')
BEGIN
	create TABLE DbReflector.ProcParams ( 
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
	create unique index bTb_procparam on DbReflector.procparams (application_name, schemaname,procname,parametername)
END
";
            public const string RefreshTable_ProcParams_10= @"
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
, dest AS (select application_name,
	SchemaName 
   	,ProcName
	,ParameterName
	,OrdinalNumber
	,Direction 
	,IsResult 
	,ParameterType  
	,ParameterMaxLen 
	,ParameterNumericPrecision 
	,ParameterDatePrecision 
	,ParameterUDTName  
from 
	DbReflector.ProcParams )

MERGE dest USING source ON source.application_name=dest.application_name and dest.schemaname=source.SPECIFIC_SCHEMA AND dest.procname=source.ROUTINE_NAME and dest.parametername=source.PARAMETER_NAME
WHEN NOT MATCHED BY TARGET THEN
INSERT (application_name, SchemaName ,ProcName ,ParameterName ,OrdinalNumber ,Direction ,IsResult ,ParameterType ,ParameterMaxLen ,ParameterNumericPrecision ,ParameterDatePrecision ,ParameterUDTName) 
values (source.application_name, source.SPECIFIC_SCHEMA, source.ROUTINE_NAME, source.PARAMETER_NAME, source.ORDINAL_POSITION, source.PARAMETER_MODE, source.IS_RESULT, source.DATA_TYPE, source.CHARACTER_MAXIMUM_LENGTH, source.NUMERIC_PRECISION, source.DATETIME_PRECISION, source.USER_DEFINED_TYPE_NAME ) 
;

delete dbreflector.ProcParams where application_name =@app and schemaname + '.' + procname + '.' +  parametername not in (
	SELECT specific_schema + '.' + specific_name +'.'+ parameter_name FROM INFORMATION_SCHEMA.parameters where specific_schema not in (<@excludedSchemasList@>))
";
            public const string CreateTable_ProcColumns_10_1 = @"
IF NOT exists(SELECT * FROM information_schema.tables WHERE table_schema=N'DbReflector' and table_name = 'ProcColumns')
BEGIN
	CREATE TABLE DbReflector.ProcColumns
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
END
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
         WHERE is_hidden = 0 AND s.name NOT IN (<@excludedSchemasList@>)),
     dest
     AS (SELECT application_name,
                schema_name, 
                proc_name, 
                ordinal_position, 
                column_name, 
                isnullable, 
                data_type, 
                character_maximum_length, 
                domain_name
         FROM DbReflector.proccolumns)
     MERGE dest
     USING source
     ON source.application_name = dest.application_name 
        AND source.schema_name = dest.schema_name
        AND source.proc_name = dest.proc_name
        AND source.column_name = dest.column_name
         WHEN MATCHED
         THEN UPDATE SET 
                         dest.schema_name = source.schema_name, 
                         dest.proc_name = source.proc_name, 
                         dest.ordinal_position = source.ordinal_position, 
                         dest.column_name = source.column_name, 
                         dest.isnullable = source.is_nullable, 
                         dest.data_type = source.data_type, 
                         dest.character_maximum_length = source.character_maximum_length, 
                         dest.domain_name = source.domain_name
         WHEN NOT MATCHED BY TARGET
         THEN
           INSERT(application_name,schema_name, 
                  proc_name, 
                  ordinal_position, 
                  column_name, 
                  isnullable, 
                  data_type, 
                  character_maximum_length, 
                  domain_name)
           VALUES
     (application_name,schema_name, 
      source.proc_name, 
      source.ordinal_position, 
      source.column_name, 
      source.is_nullable, 
      source.data_type, 
      source.character_maximum_length, 
      source.domain_name
     );
         
delete dbreflector.ProcColumns where application_name =@app and schema_name + '.' + proc_name + '.' +  column_name not in (
SELECT s.name + '.' + p.name +'.'+ isnull(r.name, 'id') 
         FROM sys.procedures AS p
              CROSS APPLY sys.dm_exec_describe_first_result_set_for_object(p.object_id, 0) AS r
              INNER JOIN sys.schemas AS s ON s.schema_id = p.schema_id
         WHERE is_hidden = 0 AND s.name not in (<@excludedSchemasList@>))
";
        }

        static class Udts
        {
            public const string CreateTable_Udts_11 = @"
IF NOT exists(SELECT * FROM information_schema.tables WHERE table_schema=N'DbReflector' and table_name = 'Udts')
BEGIN
	CREATE TABLE DbReflector.Udts
	(
        application_name nvarchar(50) not null,
		schema_name nvarchar(50) not null,
		udt_name nvarchar(150) not null,
		ignore_udt bit not null,
	)
	create unique index bTb_udts on DbReflector.udts(application_name, schema_name,udt_name)
END
";
            public const string RefreshTable_Udts_12 = @"
declare @app varchar(50) = '<@applicationName@>'
SET NOCOUNT ON
SET XACT_ABORT ON;

with source as
(
	select @app application_name, s.name schema_name, t.name udt_name,0 ignore_udt from sys.types t inner join sys.schemas s on t.schema_id=s.schema_id
	where is_user_defined = 1 and is_table_type=1 and s.name not in (<@excludedSchemasList@>)
), dest as (select application_name, schema_name, udt_name, ignore_udt from DbReflector.udts)

merge dest using source
	on source.application_name = dest.application_name and
    source.schema_name=dest.schema_name and source.udt_name = dest.udt_name
	when not matched by target then
	insert (application_name, schema_name,udt_name,ignore_udt) values (source.application_name, source.schema_name,source.udt_name,source.ignore_udt);

delete dbreflector.udts where application_name =@app and schema_name + '.' + udt_name not in (
select s.name + '.' + t.name  from sys.types t inner join sys.schemas s on t.schema_id=s.schema_id
	where is_user_defined = 1 and is_table_type=1 and s.name not in (<@excludedSchemasList@>)	)
";
            public const string CreateTable_UdtColumns_13 = @"
IF NOT exists(SELECT * FROM information_schema.tables WHERE table_schema=N'DbReflector' and table_name = 'UdtColumns')
BEGIN
	create TABLE DbReflector.UdtColumns (
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
	create unique index bTb_udtcols on DbReflector.udtcolumns (application_name,schema_name,type_name,column_name)
END

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
, dest as (
select application_name, schema_name, type_name,column_name,data_type,ordinal_number,max_length,precision,is_nullable,is_identity,is_computed from DbReflector.udtcolumns)
MERGE dest
	USING source
	ON source.application_name = dest.application_name AND source.schema_name = dest.schema_name and source.type_name= dest.type_name AND source.column_name=dest.column_name 
	WHEN MATCHED THEN
		UPDATE SET
			dest.data_type= source.data_type,
			dest.ordinal_number= source.ordinal_number,
			dest.max_length= source.max_length,
			dest.precision= source.precision,
			dest.is_nullable= source.is_nullable,
			dest.is_identity= source.is_identity,
			dest.is_computed = source.is_computed
	WHEN NOT MATCHED BY target then
		INSERT(application_name, schema_name, type_name,column_name,data_type,ordinal_number,max_length,precision,is_nullable,is_identity,is_computed)
		VALUES
		(source.application_name,source.schema_name, source.type_name, source.column_name, source.data_type,source.ordinal_number,source.max_length,source.precision,source.is_nullable,source.is_identity,source.is_computed)
;

delete dbreflector.UdtColumns where application_name =@app AND schema_name + '.'  + type_name + '.' + column_name not in (
	select 
	ss.name + '.' + t.name + '.' + c.name
from sys.table_types t
inner join sys.columns c on c.object_id = t.type_table_object_id
inner join sys.schemas ss on ss.schema_id = t.schema_id
where t.is_user_defined = 1
  and t.is_table_type = 1
  and ss.name not in (<@excludedSchemasList@>))
";
        }
        #endregion
    }
}