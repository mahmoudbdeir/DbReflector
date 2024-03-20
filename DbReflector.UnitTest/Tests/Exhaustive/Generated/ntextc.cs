
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class ntextc : BaseExhaustive
        {
            public override string ColumnName => "ntextc";
            public override string DATA_TYPE => "ntext";
            public override string CSharpType => "string";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
