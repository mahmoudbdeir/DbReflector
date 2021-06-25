
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class datetime2c : BaseExhaustive
        {
            public override string ColumnName => "datetime2c";
            public override string DATA_TYPE => "datetime2";
            public override string CSharpType => "DateTime";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
