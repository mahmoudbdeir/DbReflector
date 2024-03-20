using DbReflector.Common;

namespace DbReflector
{
    public class ProcParamInfo : IProcParamInfo
    {
        #region Constructors and Factory Methods
        ProcParamInfo(string schemaName, string procName, string parameterName, int ordinalNumber, DirectionEnum direction, bool parameterIsResult, string parameterType, int? parameterMaxLen, int? parameterNumericPrecision, int? parameterDatePrecision, string parameterUDTName)
        {
            SchemaName = schemaName;
            ProcName = procName;
            ParameterName = parameterName;
            OrdinalNumber = ordinalNumber;
            Direction = direction;
            ParameterIsResult = parameterIsResult;
            ParameterType = parameterType;
            ParameterMaxLen = parameterMaxLen;
            ParameterNumericPrecision = parameterNumericPrecision;
            ParameterDatePrecision = parameterDatePrecision;
            ParameterUDTName = parameterUDTName;
        }
        public static ProcParamInfo CreateInstance(string schemaName, string procName, string parameterName, int ordinalNumber, DirectionEnum direction, bool parameterIsResult, string parameterType, int? parameterMaxLen, int? parameterNumericPrecision, int? parameterDatePrecision, string parameterUDTName)
        => new ProcParamInfo(schemaName, procName, parameterName, ordinalNumber, direction, parameterIsResult, parameterType, parameterMaxLen, parameterNumericPrecision, parameterDatePrecision, parameterUDTName);
        #endregion

        #region Properties
        public string SchemaName { get; private set; }
        public string ProcName { get; private set; }
        public string ParameterName { get; private set; }
        public int OrdinalNumber { get; private set; }
        public DirectionEnum Direction { get; private set; }
        public bool ParameterIsResult { get; private set; }
        public string ParameterType { get; private set; }
        public int? ParameterMaxLen { get; private set; }
        public int? ParameterNumericPrecision { get; private set; }
        public int? ParameterDatePrecision { get; private set; }
        public string ParameterUDTName { get; private set; }
        #endregion
        
        public string DictionaryKey => $"{SchemaName}.{ProcName}.{ParameterName}";
    }
}