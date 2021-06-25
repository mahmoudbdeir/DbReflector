
using NUnit.Framework;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class bigintc : BaseExhaustive
    {
        public override string ColumnName => "bigintc";
        public override string DATA_TYPE => "bigint";
        public override string CSharpType => "Int64";
        public override bool IsItNullable => true;
        public override bool IsItPrimaryKey => false;
    }
}
