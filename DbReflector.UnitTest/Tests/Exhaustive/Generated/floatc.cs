
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class floatc : BaseExhaustive
        {
            public override string ColumnName => "floatc";
            public override string DATA_TYPE => "float";
            public override string CSharpType => "double";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
