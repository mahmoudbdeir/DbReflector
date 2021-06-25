using DbReflector.Common;
using DbReflector.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbReflector
{
    public class DbTableReflector : ITableReflector
    {
        DbTableReflector() { }
        public static DbTableReflector CreateInstance() => new DbTableReflector();
        public ISchemaInfo Reflect(BaseDataTableFactory factory, ISchemaInfo schemaInfo)
        {
            //DataTableFactoryFromDb DbFactory = factory as DataTableFactoryFromDb;

            // Get the Columns first.
            var TableColumns = new Dictionary<string, IColumnInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow row in factory.TablesAndViews.TableColumns.Rows)
            {
                string schema          = DataRowHelper.GetString(row,"TABLE_SCHEMA");
                string table           = DataRowHelper.GetString(row,"TABLE_NAME");
                string column          = DataRowHelper.GetString(row,"COLUMN_NAME");
                bool isPrimaryKey      = DataRowHelper.GetBool(row,"IsPrimaryKey");
                bool isForeignKey      = DataRowHelper.GetBool(row,"IsForeignKey");
                bool isKey             = DataRowHelper.GetBool(row,"IsKey");
                bool isNullable        = DataRowHelper.GetBool(row,"IsNullable");
                string dataType        = DataRowHelper.GetString(row,"DATA_TYPE");
                int? maxLength         = DataRowHelper.GetNullableInt32(row,"CHARACTER_MAXIMUM_LENGTH");
                byte? numericPrecision = DataRowHelper.GetByte(row,"NUMERIC_PRECISION");
                string domainName      = DataRowHelper.GetString(row,"DOMAIN_NAME");
                int ordinalPosition    = DataRowHelper.GetInt32(row,"ORDINAL_POSITION");

                var columnAlias     = DataRowHelper.GetString(row,"COLUMN_ALIAS");
                bool ignoreColumn    = DataRowHelper.GetBool(row,"IGNORE_COLUMN");
                if (ignoreColumn)
                    continue;

                var c = ColumnInfo.CreateIntance(
                                        schema: schema,
                                        table: table,
                                        column: column,
                                        isPrimaryKey: isPrimaryKey,
                                        isForeignKey: isForeignKey,
                                        isKey: isKey,
                                        isNullable: isNullable,
                                        dataType: dataType,
                                        maxLength: maxLength,
                                        numericPrecision: numericPrecision,
                                        domainName: domainName,
                                        ordinalPosition: ordinalPosition);
                TableColumns.Add(c.DictionaryKey, c);
            }
            schemaInfo.TableColumns = TableColumns;
            
            // Get the Tables and Views
            var TablesAndViews = new Dictionary<string, ITableInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow row in factory.TablesAndViews.TablesAndViews.Rows)
            {
                var schemaName      = DataRowHelper.GetString(row,"table_schema");
                var tableName       = DataRowHelper.GetString(row,"table_name");
                var tableType       = DataRowHelper.GetString(row,"table_type");
                var tableAlias      = DataRowHelper.GetString(row,"table_alias");

                string[] AssignedPrimaryKey = null;
                string pk = DataRowHelper.GetString(row,"primary_keys");
                if (!string.IsNullOrWhiteSpace(pk))
                {
                    AssignedPrimaryKey = pk.Split(',').Select(p => p.Trim()).ToArray();
                }

                string[] AssignedForeignKey= null;
                string fk = DataRowHelper.GetString(row,"foreign_keys");
                if (!string.IsNullOrWhiteSpace(fk))
                {
                    AssignedForeignKey = fk.Split(',').Select(p => p.Trim()).ToArray();
                }


                bool ignoreTable    = DataRowHelper.GetBool(row,"IgnoreTable");
                if (ignoreTable)
                    continue;

                var Columns = schemaInfo.TableColumns.Where(p => p.Value.SchemaName.Equals(schemaName) && p.Value.UdtName.Equals(tableName)).ToDictionary(q => q.Value.ColumnName, q => q.Value, StringComparer.InvariantCultureIgnoreCase);

                TableInfo tableMeta = TableInfo.CreateInstance(schemaName, tableName,tableType, tableAlias, AssignedPrimaryKey, AssignedForeignKey, schemaInfo.Formatter, Columns);

                tableMeta.Columns_PrimaryKeys         = tableMeta.Columns.Values.Where(p => p.IsPrimaryKey).ToList();
                tableMeta.Columns_NotPrimaryKeys      = tableMeta.Columns.Values.Where(p => p.IsPrimaryKey == false).ToList();
                tableMeta.Columns_ForeignKeys         = tableMeta.Columns.Values.Where(p => p.IsForeignKey).ToList();
                tableMeta.Columns_NotForeignKeys      = tableMeta.Columns.Values.Where(p => p.IsForeignKey == false).ToList();
                tableMeta.Columns_Keys                = tableMeta.Columns.Values.Where(p => p.IsKey).ToList();
                tableMeta.Columns_NotKeys             = tableMeta.Columns.Values.Where(p => p.IsKey == false).ToList();
                tableMeta.Columns_Nullable            = tableMeta.Columns.Values.Where(p => p.IsNullable).ToList();
                tableMeta.Columns_Nullable_NotKeys    = tableMeta.Columns.Values.Where(p => p.IsNullable && p.IsKey == false).ToList();
                tableMeta.Columns_NotNullable         = tableMeta.Columns.Values.Where(p => p.IsNullable == false).ToList();
                tableMeta.Columns_NotNullable_NotKeys = tableMeta.Columns.Values.Where(p => p.IsNullable == false && p.IsKey == false).ToList();
                tableMeta.Columns_Audit               = tableMeta.Columns.Values.Where(p => p.IsAudit).ToList();
                tableMeta.Columns_InOrder             = new List<IColumnInfo>(tableMeta.Columns_PrimaryKeys).Concat(tableMeta.Columns_ForeignKeys).Concat(tableMeta.Columns_NotKeys).ToList();
                tableMeta.ModifiedById_Column         = tableMeta.Columns.SingleOrDefault(p => p.Value.IsModifiedByUserId).Value;
                tableMeta.CreatedById_Column          = tableMeta.Columns.SingleOrDefault(p => p.Value.IsCreatedByUserId).Value;
                tableMeta.CreatedDate_Column          = tableMeta.Columns.SingleOrDefault(p => p.Value.IsCreatedDate).Value;
                tableMeta.ModifiedDate_Column         = tableMeta.Columns.SingleOrDefault(p => p.Value.IsModifiedDate).Value;

                if (!TablesAndViews.ContainsKey(tableMeta.DictionaryKey)) //in case the table has a compound primary key then add it only once
                    TablesAndViews.Add(tableMeta.DictionaryKey, tableMeta);

                //if (!Schemas.ContainsKey(schemaName))
                //{
                //    Schemas.Add(schemaName, new Dictionary<string, ITableInfo>(StringComparer.InvariantCultureIgnoreCase));
                //}
                //Schemas[schemaName].Add(tableMeta.DictionaryKey, tableMeta);
            }
            schemaInfo.TablesAndViews = TablesAndViews;

            return schemaInfo;
        }

    }
}