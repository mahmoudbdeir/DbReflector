namespace DbReflector.Common
{
    public interface IProcParamInfo : ISchemaObjectInfo
    {
        DirectionEnum Direction { get; }
        int OrdinalNumber { get; }
        int? ParameterDatePrecision { get; }
        bool ParameterIsResult { get; }
        int? ParameterMaxLen { get; }
        string ParameterName { get; }
        int? ParameterNumericPrecision { get; }
        string ParameterType { get; }
        string ParameterUDTName { get; }
        string ProcName { get; }
        string SchemaName { get; }
        
    }
    public enum DirectionEnum
    {
        In,
        Out,
        Unknown
    }
}