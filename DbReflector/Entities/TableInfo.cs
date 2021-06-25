using DbReflector.Common;
using System;
using System.Collections.Generic;

namespace DbReflector
{
    public class TableInfo : ITableInfo
    {
        const string DEFAULT_SCHEMA = "dbo";
        #region Constructors and Factory Methods

        TableInfo(string schema, string table, string tableType, string tableAlias, string[] assignedPrimaryKeys, string[] assignedForeignKeys, ITableInfoFormatter formatter, Dictionary<string, IColumnInfo> columns)
        {
            Schema = schema;
            TableName = table;
            TableType = tableType;
            TableAlias = tableAlias;
            AssignedPrimaryKeys = assignedPrimaryKeys;
            AssignedForeignKeys = assignedForeignKeys;
            this.Columns = columns;
            this.Formatter = new TableInfoFormatter(this, formatter);
            //this.Formatter.CsvColumns = formatter.CsvColumns(this);
            //this.Formatter.CsvColumnsWithAt = formatter.CsvColumnsWithAt(this);
            //this.Formatter.CsvColumnsWithAtAndType = formatter.CsvColumnsWithAtAndType(this);

        }
        internal static TableInfo CreateInstance(string schema, string table, string tableType, string tableAlias, string[] assignedPrimaryKeys, string[] assignedForeignKeys, ITableInfoFormatter formatter, Dictionary<string, IColumnInfo> columns) => new TableInfo(schema, table, tableType, tableAlias, assignedPrimaryKeys, assignedForeignKeys, formatter, columns);
        private TableInfo() //for deserialization
        {

        }
        #endregion

        #region Properties
        public string Schema { get; }
        public string TableName { get; }
        public string TableType { get; }
        public string TableAlias { get; }
        public string[] AssignedPrimaryKeys { get; }
        public string[] AssignedForeignKeys { get; }

        //public string SearchTableName { get => $"{Schema}.{TableName}".ToLower(); }
        public string TableFQN => $"{Schema}.{TableName}";
        public string TableFQNIfNotDbo => Schema.ToLower() != DEFAULT_SCHEMA ? $"{Schema}_{TableName}" : TableName;

        public string DictionaryKey => TableFQN;
        public IColumnInfo PrimaryKey => Columns_PrimaryKeys.Count > 0 ? Columns_PrimaryKeys[0] : null;
        public string NewKeyIdCsType => PrimaryKey?.CSharpType;
        public string NewKeyId => PrimaryKey != null ? $"New{PrimaryKey.ColumnName}" : "_noKey";

        #endregion
        /// <summary>
        /// DepartmentID, FirstName, GroupID, MiddleName, PersonId
        /// </summary>
        public Dictionary<string, IColumnInfo> Columns { get; internal set; }
        /// <summary>
        /// PersonId
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_PrimaryKeys { get; internal set; }
        /// <summary>
        /// PersonId
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_ForeignKeys { get; internal set; }
        /// <summary>
        /// PersonId, FirstName,MiddleName
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_NotForeignKeys { get; internal set; }
        /// <summary>
        /// FirstName,MiddleName
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_NotKeys { get; internal set; }
        /// <summary>
        /// All the columns, first the primary keys, then the foreign keys, and finally the non-key columns
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_InOrder { get; internal set; }
        /// <summary>
        /// PersonId, DepartmentId, GroupId
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_Keys { get; internal set; }
        /// <summary>
        /// FirstName,MiddleName,DepartmentId,GroupId
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_NotPrimaryKeys { get; internal set; }
        /// <summary>
        /// FirstName
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_NotNullable_NotKeys { get; internal set; }
        /// <summary>
        /// PersonId, FirstName, DepartmentId
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_NotNullable { get; internal set; }
        /// <summary>
        /// MiddleName, GroupId
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_Nullable { get; internal set; }
        /// <summary>
        /// MiddleName
        /// </summary>
        public IReadOnlyList<IColumnInfo> Columns_Nullable_NotKeys { get; internal set; }
        public IReadOnlyList<IColumnInfo> Columns_Audit { get; internal set; }
        public IColumnInfo CreatedById_Column { get; internal set; }
        public IColumnInfo ModifiedById_Column { get; internal set; }
        public IColumnInfo CreatedDate_Column { get; internal set; }
        public IColumnInfo ModifiedDate_Column { get; internal set; }

        public ITableInfoFormatterDecorator Formatter { get; }

        public override string ToString() => TableName;
    }
}