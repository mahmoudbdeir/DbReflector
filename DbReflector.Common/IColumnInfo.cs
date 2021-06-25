namespace DbReflector.Common
{
    public interface IColumnInfo : ISchemaObjectInfo
    {
        string CleanColumnName { get; }
        string CamelCaseName { get; }
        string ColumnKey { get; }
        string ColumnName { get; }
        string CSharpType { get; }
        string DataType { get; }
        string DomainName { get; }
        bool IsAudit { get; }
        bool IsCreatedByUserId { get; }
        bool IsCreatedDate { get; }
        bool IsForeignKey { get; }
        bool IsKey { get; }
        bool IsModifiedByUserId { get; }
        bool IsModifiedDate { get; }
        bool IsNullable { get; }
        bool IsPrimaryKey { get; }
        int? MaxLength { get; }
        byte? NumericPrecision { get; }
        int OrdinalPosition { get; }
        string SchemaName { get; }
        string SqlDataType { get; }
        string UdtName { get; }
    }
}