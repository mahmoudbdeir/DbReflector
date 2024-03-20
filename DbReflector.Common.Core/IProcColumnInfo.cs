namespace DbReflector.Common
{
    public interface IProcColumnInfo : ISchemaObjectInfo
    {
        string SchemaName { get; }
        string ProcName { get; }
        string ColumnName { get; }
        int OrdinalPosition { get; }
        string DataType { get; }
        bool IsNullable { get; }
        int? MaxLength { get; }
        int? NumericPrecision { get; }
    }
}
