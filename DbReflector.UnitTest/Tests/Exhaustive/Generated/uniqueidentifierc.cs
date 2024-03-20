
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("Datatype Exhaustive")]
    public class uniqueidentifierc : BaseExhaustive
        {
            public override string ColumnName => "uniqueidentifierc";
            public override string DATA_TYPE => "uniqueidentifier";
            public override string CSharpType => "Guid";
            public override bool IsItNullable => true;
            public override bool IsItPrimaryKey => false;
        }
    }
