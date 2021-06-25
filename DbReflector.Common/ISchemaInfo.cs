using System.Collections.Generic;

namespace DbReflector.Common
{
    public interface ISchemaInfo
    {
        Dictionary<string, IProcInfo> Procedures { get; set; }
        Dictionary<string, IProcParamInfo> ProcParameters { get; set; }
        Dictionary<string, IColumnInfo> TableColumns { get; set; }
        Dictionary<string, ITableInfo> Tables { get; }
        Dictionary<string, ITableInfo> TablesAndViews { get; set; }
        Dictionary<string, IUdtColumnInfo> UdtColumns { get; set; }
        Dictionary<string, IUdtInfo> UDTs { get; set; }
        Dictionary<string, ITableInfo> Views { get; }
        string CacheFile { get; set; }
        ITableInfoFormatter Formatter { get; }
        ISchemaInfo Refresh();
    }
}