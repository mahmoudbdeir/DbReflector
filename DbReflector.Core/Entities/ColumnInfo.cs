using DbReflector.Common;
using System;

namespace DbReflector
{
    public class ColumnInfo : IColumnInfo
    {
        #region Constructors and Factory Methods
        ColumnInfo(string schema, string table, string column, bool isPrimaryKey, bool isForeignKey, bool isKey, bool isNullable, string dataType, int? maxLength, byte? numericPrecision, string domainName, int ordinalPostion)
        {
            SchemaName = schema;
            UdtName = table;
            ColumnName = column;
            IsPrimaryKey = isPrimaryKey;
            IsForeignKey = isForeignKey;
            IsKey = isKey;
            IsNullable = isNullable;
            DataType = dataType;
            MaxLength = maxLength;
            NumericPrecision = numericPrecision;
            DomainName = domainName;
            OrdinalPosition = ordinalPostion;
        }
        ColumnInfo()
        {

        }
        public static ColumnInfo CreateIntance(string schema, string table, string column, bool isPrimaryKey, bool isForeignKey, bool isKey, bool isNullable, string dataType, int? maxLength, byte? numericPrecision, string domainName, int ordinalPosition) => new ColumnInfo(schema, table, column, isPrimaryKey, isForeignKey, isKey, isNullable, dataType, maxLength, numericPrecision, domainName, ordinalPosition);
        #endregion

        #region Properties
        public string SchemaName { get; private set; }
        public string UdtName { get; private set; }

        public string ColumnName { get; }
        public string CleanColumnName => Clean(ColumnName);
        public bool IsNullable { get; private set; }
        public string DataType { get; private set; }
        public string DomainName { get; private set; }
        public bool IsPrimaryKey { get; private set; }
        public bool IsForeignKey { get; private set; }
        public bool IsKey { get; private set; }
        public int? MaxLength { get; private set; }
        public byte? NumericPrecision { get; private set; }
        public int OrdinalPosition { get; private set; }
        #endregion
        public string ColumnKey => $"{UdtName}.{ColumnName}";
        public string DictionaryKey => $"{SchemaName}.{UdtName}.{ColumnName}";

        #region Audit Columns
        public bool IsCreatedByUserId => ColumnName.Equals("createdbyuserid", StringComparison.InvariantCultureIgnoreCase);
        public bool IsCreatedDate => ColumnName.Equals("createddate", StringComparison.InvariantCultureIgnoreCase);
        public bool IsModifiedByUserId => ColumnName.Equals("modifiedbyuserid", StringComparison.InvariantCultureIgnoreCase);
        public bool IsModifiedDate => ColumnName.Equals("modifieddate", StringComparison.InvariantCultureIgnoreCase);
        public bool IsAudit => IsCreatedByUserId || IsCreatedDate || IsModifiedByUserId || IsModifiedDate;
        #endregion

        static string Clean(string s) { return s.Replace(" ", "_"); }
        public string CSharpType => SqlToCSharpType(DataType);
        public static string SqlToCSharpType(string s)
        {
            switch (s)
            {
                //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/sql-clr-type-mapping#NumericMapping
                #region Numeric Mapping
                case "bit":
                    return "bool";
                case "tinyint":
                    return "byte";
                case "smallint":
                    return "Int16";
                case "int":
                    return "Int32";
                case "bigint":
                    return "Int64";

                case "smallmoney":
                case "money":
                case "decimal":
                case "numeric":
                    return "decimal";

                case "real":
                    return "Single"; 
                case "float":
                    return "double"; 
                #endregion

                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    return "string";
                case "xml":
                    return "System.Xml.Linq.XElement";

                case "smalldatetime":
                case "datetime":
                case "datetime2":
                case "date":
                    return "DateTime";
                case "datetimeoffset":
                    return "System.DateTimeOffset";
                case "time":
                    return "TimeSpan";

                case "binary":
                case "varbinary":
                case "image":
                case "timestamp":
                    return "byte[]";

                case "uniqueidentifier":
                    return "Guid";
                case "Sql_variant":
                    return "Object";
                default:
                    throw new Exception($"MHB: (SqlToCSharpType) I need to construct the SQL Type for {s}");
            }
        }
        public string SqlDataType
        {
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings

            get
            {
                string SqlTypeName = DataType.ToUpper();
                switch (DataType.ToLower())
                {
                    //case "tinyint":
                    //    return "byte";
                    case "int":
                    case "bigint":
                    case "uniqueidentifier":
                    case "ntext":
                    case "bit":
                    case "datetime2":
                    case "datetime":
                    case "date":
                    case "money":
                    case "xml":
                    case "smallint":
                    case "hierarchyid":
                    case "time":
                    case "decimal":
                    case "numeric":
                    case "smallmoney":
                    case "float":
                        return SqlTypeName;
                    case "tinyint":
                        return "Int16";
                    case "binary":
                        return "byte[]";
                    case "datetimeoffset":
                        return "DateTimeOffset";
                    case "real":
                        return "Single";
                    case "smalldatetime":
                        return "DateTime";
                    case "text":
                        return "string";
                    case "timestamp":
                        return "Timestamp";
                    case "nvarchar":
                    case "varchar":
                    case "varbinary":
                    case "char":
                    case "nchar":
                        return $"{SqlTypeName}({MaxLength})";
                    case "geography":
                        return SqlTypeName;
                    default:
                        throw new System.Exception($"MHB: (SqlDataType) I need to construct the SQL Type for {DataType}");
                }
            }
        }

        public string CamelCaseName => ColumnName[0].ToString().ToLower() + ColumnName.Substring(1);

        public override string ToString() => ColumnName;
    }
}