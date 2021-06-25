using System;
using System.Data;

namespace DbReflector
{
    public static class DataRowHelper
    {
        public static int GetInt32(DataRow row, string columnName) => int.Parse(row[columnName].ToString());
        public static byte? GetByte(DataRow row, string columnName) => row[columnName] == DBNull.Value ? null : (byte?)byte.Parse(row[columnName].ToString());
        public static string GetString(DataRow row, string columnName) => row[columnName].ToString();
        public static bool GetBool(DataRow row, string columnName) => bool.Parse(row[columnName].ToString());
        public static int? GetNullableInt32(DataRow row, string columnName) => row[columnName] == DBNull.Value ? null : (int?)int.Parse(row[columnName].ToString());
    }
}
