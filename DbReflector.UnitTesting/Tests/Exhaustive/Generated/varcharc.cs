
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class varcharc : BaseExhaustive
        {
            public override string ColumnName => "varcharc";
            public override string DATA_TYPE => "varchar";
            public override string CSharpType => "string";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
