using System.Collections.Generic;

namespace DbReflector.Common
{
    public interface IUdtInfo : ISchemaObjectInfo
    {
        Dictionary<string, IUdtColumnInfo> Columns { get; }
        string Database { get; }
        string Schema { get; }
        string UdtName { get; }
    }
}