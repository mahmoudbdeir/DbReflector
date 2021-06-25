
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class ncharc : BaseExhaustive
        {
            public override string ColumnName => "ncharc";
            public override string DATA_TYPE => "nchar";
            public override string CSharpType => "string";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
