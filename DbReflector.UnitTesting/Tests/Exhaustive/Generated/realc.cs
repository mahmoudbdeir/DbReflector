
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class realc : BaseExhaustive
        {
            public override string ColumnName => "realc";
            public override string DATA_TYPE => "real";
            public override string CSharpType => "Single";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
