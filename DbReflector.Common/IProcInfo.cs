using System.Collections.Generic;

namespace DbReflector.Common
{
    public interface IProcInfo : ISchemaObjectInfo
    {
        Dictionary<string, IProcParamInfo> Parameters { get; }
        Dictionary<string, IProcColumnInfo> Columns { get; }
        string ProcName { get; }
        string ProcSchema { get; }
        IProcParamInfo ReturnParameter { get; }
        bool ReturnsTable { get; }
        bool ReturnsOutputParameters { get; }
        string SourceCode { get; }
    }
}