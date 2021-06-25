
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class bitc : BaseExhaustive
        {
            public override string ColumnName => "bitc";
            public override string DATA_TYPE => "bit";
            public override string CSharpType => "bool";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
