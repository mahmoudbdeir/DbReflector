
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class id : BaseExhaustive
        {
            public override string ColumnName => "id";
            public override string DATA_TYPE => "int";
            public override string CSharpType => "Int32";
            public override bool IsItNullable => false;
            public override bool IsItPrimaryKey => true;
        }
    }
