using DbReflector.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbReflector
{
    public class SchemaInfo : ISchemaInfo
    {
        public string ConnectionString { get; private set; }
        public SchemaInfo(string connectionString, ITableInfoFormatter formatter)
        {
            ConnectionString = connectionString;
            this.Formatter = formatter;
        }
        public Dictionary<string, IProcParamInfo> ProcParameters { get; set; }
        public Dictionary<string, IProcInfo> Procedures { get; set; }
        public Dictionary<string, IColumnInfo> TableColumns { get; set; }
        public Dictionary<string, IUdtColumnInfo> UdtColumns { get; set; }
        public Dictionary<string, ITableInfo> TablesAndViews { get; set; }
        public Dictionary<string, IUdtInfo> UDTs { get; set; }
        public string CacheFile { get; set; }
        #region Tables
        Dictionary<string, ITableInfo> _Tables;
        public Dictionary<string, ITableInfo> Tables
        {
            get
            {
                if (_Tables == null)
                {
                    _Tables = TablesAndViews.Where(p => p.Value.TableType == "BASE TABLE").ToDictionary(p => p.Key, p => p.Value, StringComparer.InvariantCultureIgnoreCase);
                }
                return _Tables;
            }
        }
        #endregion

        #region Views
        Dictionary<string, ITableInfo> _Views;
        public Dictionary<string, ITableInfo> Views
        {
            get
            {
                if (_Views == null)
                {
                    _Views = TablesAndViews.Where(p => p.Value.TableType == "VIEW").ToDictionary(p => p.Key, p => p.Value, StringComparer.InvariantCultureIgnoreCase);
                }
                return _Views;
            }
        }


        #endregion

        public ISchemaInfo Refresh()
        {
            Procedures = null;
            ProcParameters = null;
            TableColumns = null;
            TablesAndViews = null;
            UdtColumns = null;
            UDTs = null;
            return this;
        }
        public ITableInfoFormatter Formatter { get; }

        
    }
}