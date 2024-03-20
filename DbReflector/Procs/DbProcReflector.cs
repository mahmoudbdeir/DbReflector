using DbReflector.Common;
using DbReflector.Entities;
using DbReflector.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DbReflector
{
    public class DbProcReflector : IProcReflector
    {
        DbProcReflector(){}
        public static DbProcReflector CreateInstance() => new DbProcReflector();
        public ISchemaInfo Reflect(BaseDataTableFactory factory, ISchemaInfo schemaInfo)
        {
            //DataTableFactoryFromDb DbFactory = factory as DataTableFactoryFromDb;

            // Get the Parameters first.
            var ProcParameters = new Dictionary<string, IProcParamInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow row in factory.Procs.ProcParams.Rows)
            {
                string schemaName    = DataRowHelper.GetString(row,"SchemaName");
                string procName      = DataRowHelper.GetString(row,"ProcName");
                string parameterName = DataRowHelper.GetString(row, "ParameterName");
                int ordinalNumber    = DataRowHelper.GetInt32(row,"OrdinalNumber");
                DirectionEnum direction = DirectionEnum.Unknown;

                switch (DataRowHelper.GetString(row, "Direction").ToLower())
                {
                    case "in":
                        direction = DirectionEnum.In;
                        break;
                    case "inout":
                        direction = DirectionEnum.Out;
                        break;
                    default:
                        throw new Exception($"DbProcReflector::Reflect() => Unexpected value for [Parameter Direction]. The value was: {direction}");
                }

                bool parameterIsResult         = DataRowHelper.GetBool(row,"IsResult");
                string parameterType           = DataRowHelper.GetString(row,"ParameterType");
                int? parameterMaxLen           = DataRowHelper.GetNullableInt32(row,"ParameterMaxLen");
                int? parameterNumericPrecision = DataRowHelper.GetNullableInt32(row,"ParameterNumericPrecision");
                int? parameterDatePrecision    = DataRowHelper.GetNullableInt32(row,"ParameterDatePrecision");
                string parameterUDTName        = DataRowHelper.GetString(row,"ParameterUDTName");
                var parameter = ProcParamInfo.CreateInstance(
                                        schemaName: schemaName,
                                        procName: procName,
                                        parameterName: parameterName,
                                        ordinalNumber: ordinalNumber,
                                        direction: direction,
                                        parameterIsResult: parameterIsResult,
                                        parameterType: parameterType,
                                        parameterMaxLen: parameterMaxLen,
                                        parameterNumericPrecision: parameterNumericPrecision,
                                        parameterDatePrecision: parameterDatePrecision,
                                        parameterUDTName: parameterUDTName);
                ProcParameters.Add(parameter.DictionaryKey, parameter);
            }
            schemaInfo.ProcParameters = ProcParameters;

            //Get the columns second
            var ProcColumns = new Dictionary<string, IProcColumnInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow row in factory.Procs.ProcColumns.Rows)
            {
                string schemaName           = DataRowHelper.GetString(row, "schema_name");
                string procName             = DataRowHelper.GetString(row, "proc_name");
                int ordinalNumber           = DataRowHelper.GetInt32(row,"ordinal_position");
                string columnName           = DataRowHelper.GetString(row,"column_name");
                bool isNullable             = DataRowHelper.GetBool(row,"isnullable");
                
                string type                 = DataRowHelper.GetString(row,"data_type");
                int index                   = type.IndexOf('(');
                type                        = index < 0 ? type : type.Substring(0, index);

                string columnType           = type;
                int? columnMaxLen           = DataRowHelper.GetNullableInt32(row,"character_maximum_length");
                string columnDomain         = DataRowHelper.GetString(row,"DOMAIN_NAME");

                var column = ProcColumnInfo.CreateInstance(schemaName, columnName, columnType, isNullable, columnMaxLen, null, ordinalNumber, procName);
                ProcColumns.Add(column.DictionaryKey, column);
            }
            


            // Get the Procs
            var Procedures = new Dictionary<string, IProcInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow row in factory.Procs.Procs.Rows)
            {
                string schemaName = DataRowHelper.GetString(row,"SchemaName");
                string procName = DataRowHelper.GetString(row,"ProcName");
                bool ignoreProc = DataRowHelper.GetBool(row,"IgnoreProc");
                string sourceCode = DataRowHelper.GetString(row,"SourceCode");
                
                if (ignoreProc)
                    continue;
                ProcInfo procInfo = ProcInfo.CreateInstance(schemaName, procName, sourceCode);
                
                procInfo.Parameters = schemaInfo.ProcParameters.Where(p => p.Value.ProcName == procName && p.Value.SchemaName == schemaName).ToDictionary(p => p.Value.ParameterName, p => p.Value, StringComparer.InvariantCultureIgnoreCase);

                procInfo.Columns = ProcColumns.Where(p => p.Value.ProcName.Equals(procName, StringComparison.InvariantCultureIgnoreCase)).ToDictionary(p => p.Value.DictionaryKey, p => p.Value, StringComparer.InvariantCultureIgnoreCase);

                Procedures.Add(procInfo.DictionaryKey, procInfo);
            }
            schemaInfo.Procedures = Procedures;
            return schemaInfo;
        }
}
}

