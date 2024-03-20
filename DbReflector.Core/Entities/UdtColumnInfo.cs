using DbReflector.Common;

namespace DbReflector
{
    public class UdtColumnInfo : IUdtColumnInfo
    {
        #region Constructors and Factory Methods
        UdtColumnInfo(string table, string column, bool isNullable, string dataType, int? maxLength, int? numericPrecision, int ordinalPostion)
        {
            UdtName = table;
            ColumnName = column;
            IsNullable = isNullable;
            DataType = dataType;
            MaxLength = maxLength;
            NumericPrecision = numericPrecision;
            OrdinalPosition = ordinalPostion;
        }
        public static UdtColumnInfo CreateIntance(string table, string column, bool isNullable, string dataType, int? maxLength, int? numericPrecision, int ordinalPosition) => new UdtColumnInfo(table, column, isNullable, dataType, maxLength, numericPrecision, ordinalPosition);
        #endregion

        #region Properties
        public string UdtName { get; private set; }
        public string ColumnName { get; }
        public bool IsNullable { get; private set; }
        public string DataType { get; private set; }
        public int? MaxLength { get; private set; }
        public int? NumericPrecision { get; private set; }
        public int OrdinalPosition { get; private set; }
        #endregion
        public string ColumnKey => $"{UdtName}.{ColumnName}";
        public string DictionaryKey => $"{UdtName}.{ColumnName}";
    }
}