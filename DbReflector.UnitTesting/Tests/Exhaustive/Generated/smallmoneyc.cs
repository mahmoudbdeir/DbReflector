
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class smallmoneyc : BaseExhaustive
        {
            public override string ColumnName => "smallmoneyc";
            public override string DATA_TYPE => "smallmoney";
            public override string CSharpType => "decimal";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
