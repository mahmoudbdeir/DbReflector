using DbReflector.Common;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DbReflector
{
    public class CSharpTableInfoFormatter : ITableInfoFormatter
    {
        CSharpTableInfoFormatter(){}
        public static CSharpTableInfoFormatter CreateInstance() => new CSharpTableInfoFormatter();
        /// <summary>
        /// Returns a CSV string for all columns. Note: MaterialID is the primary key in the 'returns' example.
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>For table Material: MaterialID, ActionID, Cost, Manufacturer, PartDescription, PartNumber, Vendor</returns>
        public string CsvColumns(ITableInfo tableInfo) => _CsvColumns(tableInfo.Columns_InOrder.ToList());

        /// <summary>
        /// Returns a CSV string for all columns except the primary key.
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>For table Material: ActionID, Cost, Manufacturer, PartDescription, PartNumber, Vendor</returns>
        public string CsvColumnsNoPk(ITableInfo tableInfo) => _CsvColumns(tableInfo.Columns_NotPrimaryKeys.ToList());

        /// <summary>
        /// Returns a CSV string for all columns where each column is preceded by @.
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>For table Material: @MaterialID, @ActionID, @Cost, @Manufacturer, @PartDescription, @PartNumber, @Vendor</returns>
        public string CsvColumnsWithAt(ITableInfo tableInfo) => _CsvColumnsWithAt(tableInfo.Columns_InOrder.ToList());

        /// <summary>
        /// Returns a CSV string for all columns where each column is preceded by @.
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>For table Material: @ActionID, @Cost, @Manufacturer, @PartDescription, @PartNumber, @Vendor</returns>
        public string CsvColumnsNoPkWithAt(ITableInfo tableInfo) => _CsvColumnsWithAt(tableInfo.Columns_NotPrimaryKeys.ToList());

        /// <summary>
        /// Returns a CSV string for all columns where each column is preceded by @ and followed by the SQL Server data type.
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>For table Material: @MaterialID INT, @ActionID INT, @Cost MONEY, @Manufacturer VARCHAR(100), @PartDescription VARCHAR(100), @PartNumber VARCHAR(25), @Vendor VARCHAR(50)</returns>
        public string CsvColumnsWithAtAndSqlType(ITableInfo tableInfo)
        {
            var a = tableInfo.Columns_InOrder.ToList();
            return _CsvColumnsWithAtAndSqlType(tableInfo.Columns_InOrder.ToList());
        }

        /// <summary>
        /// Returns a CSV string for all columns except the primary key where each column is preceded by @ and followed by the SQL Server data type.
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>@ActionID INT, @Cost MONEY, @Manufacturer VARCHAR(100), @PartDescription VARCHAR(100), @PartNumber VARCHAR(25), @Vendor VARCHAR(50)</returns>
        public string CsvColumnsNoPkWithAtAndSqlType(ITableInfo tableInfo) => _CsvColumnsWithAtAndSqlType(tableInfo.Columns_NotPrimaryKeys.ToList());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        public string CSharpFuncParams(ITableInfo tableInfo) => _CSharpFuncParams(tableInfo.Columns_InOrder.ToList());

        public string CsvColumnsWithAtAndAlias(ITableInfo tableInfo) => _CsvColumnsWithAtAndAlias(tableInfo.Columns_InOrder.ToList());

        public string CSharpFuncInvokeParams(ITableInfo tableInfo) => _CSharpFuncInvokeParams(tableInfo);


        #region Private Helpers
        static List<char> chars = new List<char>() {' ', ','};  
        static string CleanSb(StringBuilder sb)
        {

            while (chars.Contains(sb[sb.Length - 1])) sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        static string _CsvColumnsWithAtAndSqlType(List<IColumnInfo> columns)
        {
            var sb = new StringBuilder();
            foreach (var column in columns)
            {
                sb.Append($"@{column.ColumnName} {column.SqlDataType}, ");
            }
            return CleanSb(sb);
        }

        static string _CSharpFuncParams(List<IColumnInfo> columns)
        {
            var sb = new StringBuilder();
            foreach (var column in columns)
            {
                sb.Append($"{column.CSharpType}{(((column.CSharpType != "string" && column.CSharpType != "byte[]") && column.IsNullable) ? "?" : "")} @{column.CamelCaseName}, ");
            }
            return CleanSb(sb);
        }

        static string _CSharpFuncInvokeParams(ITableInfo tableInfo)
        {
            var columns = tableInfo.Columns_InOrder;
            var sb = new StringBuilder();
            foreach (var column in columns)
            {
                sb.Append($"{tableInfo.Formatter.CSharpEntityNameCamelCase}.{column.ColumnName}, ");
            }
            return CleanSb(sb);
        }

        static string _CsvColumns(List<IColumnInfo> columns)
        {
            var sb = new StringBuilder();
            foreach (var item in columns)
            {
                sb.Append($"{item.ColumnName}, ");
            }
            return CleanSb(sb);
        }

        static string _CsvColumnsWithAt(List<IColumnInfo> columns)
        {
            var sb = new StringBuilder();
            foreach (var item in columns)
            {
                sb.Append($"@{item.ColumnName}, ");
            }
            return CleanSb(sb);
        }

        static string _CsvColumnsWithAtAndAlias(List<IColumnInfo> columns)
        {
            var sb = new StringBuilder();
            foreach (var item in columns)
            {
                sb.Append($"@{item.ColumnName} {item.ColumnName}, ");
            }
            return CleanSb(sb);
        }

        public string CSharpEntityNameCamelCase(ITableInfo tableInfo) => tableInfo.TableName[0].ToString().ToLower() + tableInfo.TableName.Substring(1);

        #endregion
    }
}