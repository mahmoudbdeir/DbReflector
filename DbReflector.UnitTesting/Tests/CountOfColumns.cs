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
            Assert.AreEqual(9, table.Columns.Count);
        }
        [Test]
        public void NotNullCount()
        {
            Assert.AreEqual(7, table.Columns_NotNullable.Count);
        }
        [Test]
        public void NullCount()
        {
            Assert.AreEqual(2, table.Columns_Nullable.Count);
        }
        [Test]
        public void PrimaryKeysCount()
        {
            Assert.AreEqual(1, table.Columns_PrimaryKeys.Count);
        }
        [Test]
        public void NotPrimaryKeysCount()
        {
            Assert.AreEqual(8, table.Columns_NotPrimaryKeys.Count);
        }
        [Test]
        public void ForeignKeysCount()
        {
            Assert.AreEqual(1, table.Columns_ForeignKeys.Count);
        }

        [Test]
        public void NotForeignKeysCount()
        {
            Assert.AreEqual(8, table.Columns_NotForeignKeys.Count);
        }
        [Test]
        public void KeysCount()
        {
            Assert.AreEqual(2, table.Columns_Keys.Count);
        }
        [Test]
        public void NotKeysCount()
        {
            Assert.AreEqual(7, table.Columns_NotKeys.Count);
        }

        [Test]
        public void InOrderColumnCount()
        {
            Assert.AreEqual(9, table.Columns_InOrder.Count);
        }
        [Test]
        public void Columns_NotNullable_NotKeys()
        {
            Assert.AreEqual(5, table.Columns_NotNullable_NotKeys.Count);
        }
        [Test]
        public void Columns_Audit()
        {
            Assert.AreEqual(1, table.Columns_Audit.Count);
        }
    }
}