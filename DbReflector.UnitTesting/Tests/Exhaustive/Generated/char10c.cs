
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class char10c : BaseExhaustive
        {
            public override string ColumnName => "char10c";
            public override string DATA_TYPE => "char";
            public override string CSharpType => "string";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
