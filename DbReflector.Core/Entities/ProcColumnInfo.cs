using DbReflector.Common;

namespace DbReflector.Entities
{
    public class ProcColumnInfo : IProcColumnInfo
    {
        ProcColumnInfo(string schemaName, string columnName, string dataType, bool isNullable, int? maxLength, int? numericPrecision, int ordinalPosition, string procName)
        {
            SchemaName = schemaName;
            ColumnName = columnName;
            DataType = dataType;
            IsNullable = isNullable;
            MaxLength = maxLength;
            NumericPrecision = numericPrecision;
            OrdinalPosition = ordinalPosition;
            ProcName = procName;
        }
        public static ProcColumnInfo CreateInstance(string schemaName, string columnName, string dataType, bool isNullable, int? maxLength, int? numericPrecision, int ordinalPosition, string procName) => new ProcColumnInfo(schemaName, columnName, dataType, isNullable, maxLength, numericPrecision, ordinalPosition, procName);

        public string ColumnName { get; private set; }
        public string SchemaName { get; private set; }

        public string DataType { get; private set; }

        public string DictionaryKey => $"{SchemaName}.{ProcName}.{ColumnName}";

        public bool IsNullable { get; private set; }

        public int? MaxLength { get; private set; }

        public int? NumericPrecision { get; private set; }

        public int OrdinalPosition { get; private set; }

        public string ProcName { get; private set; }
    }
}
