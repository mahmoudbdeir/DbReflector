using DbReflector.Common;
using NUnit.Framework;

namespace DbReflector.UnitTesting.Exhaustive
{
    [Category("AdventureWorksCs")]
    public abstract class BaseExhaustive
    {
        static ISchemaInfo schema = Globals.AdventureWorksCs.ShemaInfo;
        TableInfo t;
        IColumnInfo c;

        public const string TableName = "dbo.UnitTest";
        public const int NumOfColumns = 30;

        public abstract string ColumnName { get; }
        public abstract string DATA_TYPE { get;}
        public abstract string CSharpType { get; }
        public abstract bool IsItNullable { get; }
        public abstract bool IsItPrimaryKey { get; }

        [SetUp]
        public void Setup()
        {
            t = schema.Tables[TableName] as TableInfo;
            c = t.Columns[ColumnName];
        }

        //[Test]
        //public void GetByName()
        //{
        //    Assert.That(t, $"Could not find {TableName}");
        //}

        //[Test]
        //public void CountOfColumns()
        //{
        //    Assert.Equals(NumOfColumns, t.Columns.Count);
        //}

        //[Test]
        //public void GetColumnByName()
        //{
        //    Assert.That(c);
        //}

        //[Test]
        //public void PrimaryKey()
        //{
        //    Assert.Equals(IsItPrimaryKey, c.IsPrimaryKey);
        //}


        //[Test]
        //public void DataType()
        //{
        //    Assert.Equals(DATA_TYPE, c.DataType);
        //}

        //[Test]
        //public void CsharpDataType()
        //{
        //    Assert.Equals(CSharpType, c.CSharpType, $"{this.GetType().Name} {this.ColumnName}");
        //}

        //[Test]
        //public void IsNullable()
        //{
        //    Assert.Equals(IsItNullable, c.IsNullable);
        //}
    }
}
