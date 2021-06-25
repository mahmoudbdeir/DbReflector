
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class moneyc : BaseExhaustive
        {
            public override string ColumnName => "moneyc";
            public override string DATA_TYPE => "money";
            public override string CSharpType => "decimal";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
