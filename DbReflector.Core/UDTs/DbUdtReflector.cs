using DbReflector.Common;
using DbReflector.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbReflector
{
    public class DbUdtReflector : IUdtReflector
    {
        DbUdtReflector(){}
        public static DbUdtReflector CreateInstance() => new DbUdtReflector();
        public ISchemaInfo Reflect(BaseDataTableFactory factory, ISchemaInfo schemaInfo)
        {
            //DataTableFactoryFromDb DbFactory = factory as DataTableFactoryFromDb;

            // Get the Columns first.
            var UdtColumns = new Dictionary<string, IUdtColumnInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow row in factory.Udts.UdtColumns.Rows)
            {
                string udt            = DataRowHelper.GetString(row,"type_name");
                string column         = DataRowHelper.GetString(row,"column_name");
                int ordinalPosition   = DataRowHelper.GetInt32(row,"ordinal_number");
                string dataType       = DataRowHelper.GetString(row,"type_name");
                int? maxLength        = DataRowHelper.GetNullableInt32(row,"max_length");
                int? numericPrecision = DataRowHelper.GetNullableInt32(row,"precision");
                bool isNullable       = DataRowHelper.GetBool(row,"is_nullable");

                var c = UdtColumnInfo.CreateIntance(
                                        table: udt,
                                        column: column,
                                        isNullable: isNullable,
                                        dataType: dataType,
                                        maxLength: maxLength,
                                        numericPrecision: numericPrecision,
                                        ordinalPosition: ordinalPosition);
                UdtColumns.Add(c.DictionaryKey, c);
            }
            schemaInfo.UdtColumns = UdtColumns;

            var UDTs = new Dictionary<string, IUdtInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow row in factory.Udts.Udts.Rows)
            {
                string databaseName = "not set"; //todo get the database name
                string schemaName   = DataRowHelper.GetString(row,"schema_name");
                var udtName         = DataRowHelper.GetString(row,"udt_name");
                UdtInfo udtMeta = UdtInfo.CreateInstance(databaseName, schemaName, udtName);
                udtMeta.Columns = schemaInfo.UdtColumns.Where(p => p.Value.UdtName.Equals(udtName)).ToDictionary(q => q.Value.ColumnName, q => q.Value, StringComparer.InvariantCultureIgnoreCase);
                UDTs.Add(udtMeta.DictionaryKey, udtMeta);
            }
            schemaInfo.UDTs = UDTs;
            return schemaInfo;
        }
    }
}
