
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class datetimec : BaseExhaustive
        {
            public override string ColumnName => "datetimec";
            public override string DATA_TYPE => "datetime";
            public override string CSharpType => "DateTime";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
