
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class datec : BaseExhaustive
        {
            public override string ColumnName => "datec";
            public override string DATA_TYPE => "date";
            public override string CSharpType => "DateTime";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
