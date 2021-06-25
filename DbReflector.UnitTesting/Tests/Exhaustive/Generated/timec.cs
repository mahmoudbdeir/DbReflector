
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class timec : BaseExhaustive
        {
            public override string ColumnName => "timec";
            public override string DATA_TYPE => "time";
            public override string CSharpType => "TimeSpan";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
