
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class pk_idc : BaseExhaustive
        {
            public override string ColumnName => "pk_idc";
            public override string DATA_TYPE => "int";
            public override string CSharpType => "Int32";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
