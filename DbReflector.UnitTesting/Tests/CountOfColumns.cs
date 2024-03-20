using DbReflector.Common;
using NUnit.Framework;

namespace DbReflector.UnitTesting
{
    [Category("AdventureWorksCs")]
    public class CountOfColumns
    {
        ISchemaInfo schema;
        TableInfo table;
        
        [OneTimeSetUp]
        public void Setup()
        {
            schema = Globals.AdventureWorksCs.ShemaInfo;
            table = schema.Tables["Person.Address"] as TableInfo;
        }
        
        [Test]
        public void ColumnCount()
        {
            Assert.Equals(9, table.Columns.Count);
        }
        [Test]
        public void NotNullCount()
        {
            Assert.Equals(7, table.Columns_NotNullable.Count);
        }
        [Test]
        public void NullCount()
        {
            Assert.Equals(2, table.Columns_Nullable.Count);
        }
        [Test]
        public void PrimaryKeysCount()
        {
            Assert.Equals(1, table.Columns_PrimaryKeys.Count);
        }
        [Test]
        public void NotPrimaryKeysCount()
        {
            Assert.Equals(8, table.Columns_NotPrimaryKeys.Count);
        }
        [Test]
        public void ForeignKeysCount()
        {
            Assert.Equals(1, table.Columns_ForeignKeys.Count);
        }

        [Test]
        public void NotForeignKeysCount()
        {
            Assert.Equals(8, table.Columns_NotForeignKeys.Count);
        }
        [Test]
        public void KeysCount()
        {
            Assert.Equals(2, table.Columns_Keys.Count);
        }
        [Test]
        public void NotKeysCount()
        {
            Assert.Equals(7, table.Columns_NotKeys.Count);
        }

        [Test]
        public void InOrderColumnCount()
        {
            Assert.Equals(9, table.Columns_InOrder.Count);
        }
        [Test]
        public void Columns_NotNullable_NotKeys()
        {
            Assert.Equals(5, table.Columns_NotNullable_NotKeys.Count);
        }
        [Test]
        public void Columns_Audit()
        {
            Assert.Equals(1, table.Columns_Audit.Count);
        }
    }
}