using DbReflector.Common;
using System.Collections.Generic;

namespace DbReflector
{
    public class UdtInfo : IUdtInfo
    {
        #region Constructors and Factory Methods
        UdtInfo(string database, string schema, string udt)
        {
            Database = database;
            Schema = schema;
            UdtName = udt;
        }
        internal static UdtInfo CreateInstance(string database, string schema, string udt) => new UdtInfo(database, schema, udt);
        #endregion

        #region Properties
        public string Database { get; }
        public string Schema { get; }
        public string UdtName { get; }
        #endregion
        public Dictionary<string, IUdtColumnInfo> Columns { get; internal set; }
        public string DictionaryKey => $"{Schema}.{UdtName}";
    }
}