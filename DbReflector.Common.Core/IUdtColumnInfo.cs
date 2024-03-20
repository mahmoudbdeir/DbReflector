namespace DbReflector.Common
{
    public interface IUdtColumnInfo : ISchemaObjectInfo
    {
        string ColumnKey { get; }
        string ColumnName { get; }
        string DataType { get; }
        bool IsNullable { get; }
        int? MaxLength { get; }
        int? NumericPrecision { get; }
        int OrdinalPosition { get; }
        string UdtName { get; }
    }
}