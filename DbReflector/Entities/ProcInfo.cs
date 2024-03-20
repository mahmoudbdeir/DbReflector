using DbReflector.Common;
using System.Collections.Generic;
using System.Linq;

namespace DbReflector
{
    public class ProcInfo : IProcInfo
    {
        #region Constructors and Factory Methods
        ProcInfo(string schema, string proc, string sourceCode)
        {
            this.ProcSchema = schema;
            this.ProcName = proc;
            this.SourceCode = sourceCode;
        }
        public static ProcInfo CreateInstance(string schema, string proc, string sourceCode) => new ProcInfo(schema, proc, sourceCode);
        #endregion
        public string ProcSchema { get; private set; }
        public string ProcName { get; private set; }
        public Dictionary<string, IProcParamInfo> Parameters { get; internal set; }
        public Dictionary<string, IProcColumnInfo> Columns { get; internal set; }
        public IProcParamInfo ReturnParameter { get; internal set; }
        public string DictionaryKey => $"{ProcSchema}.{ProcName}";

        public bool ReturnsOutputParameters => Parameters.Any(p => p.Value.Direction == DirectionEnum.Out);

        public bool ReturnsTable => Columns.Count > 0;

        public string SourceCode { get; internal set; }

        public override string ToString() => $"{ProcSchema}.{ProcName}";
    }
}