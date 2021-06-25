
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class varbinaryc : BaseExhaustive
        {
            public override string ColumnName => "varbinaryc";
            public override string DATA_TYPE => "varbinary";
            public override string CSharpType => "byte[]";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
