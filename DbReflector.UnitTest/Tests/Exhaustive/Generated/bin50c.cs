
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class bin50c : BaseExhaustive
        {
            public override string ColumnName => "bin50c";
            public override string DATA_TYPE => "binary";
            public override string CSharpType => "byte[]";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
