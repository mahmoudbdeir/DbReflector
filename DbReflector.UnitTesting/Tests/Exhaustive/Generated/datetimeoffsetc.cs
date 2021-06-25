
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class datetimeoffsetc : BaseExhaustive
        {
            public override string ColumnName => "datetimeoffsetc";
            public override string DATA_TYPE => "datetimeoffset";
            public override string CSharpType => "System.DateTimeOffset";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
