
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class smallintc : BaseExhaustive
        {
            public override string ColumnName => "smallintc";
            public override string DATA_TYPE => "smallint";
            public override string CSharpType => "Int16";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
