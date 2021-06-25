
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class decimalc : BaseExhaustive
        {
            public override string ColumnName => "decimalc";
            public override string DATA_TYPE => "decimal";
            public override string CSharpType => "decimal";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
