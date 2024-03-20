using DbReflector;
using DbReflector.Common;
using NUnit.Framework;

namespace DbReflector.UnitTesting
{
    [Category("AdventureWorksCs")]
    public class PresenceOfColumns
    {
        ISchemaInfo schema = Globals.AdventureWorksCs.ShemaInfo;
        TableInfo table;
        [SetUp]
        public void Setup()
        {
            table = schema.Tables["person.address"] as TableInfo;
        }

        [Test]
        public void AddressLine1()
        {
            Assert.That(table.Columns["AddressLine1"] != null, "Could not find column AddressLine1");
        }

        [Test]
        public void AddressLine2()
        {
            Assert.That(table.Columns["AddressLine2"] != null, "Could not find column AddressLine2");
        }

        [Test]
        public void City()
        {
            Assert.That(table.Columns["City"] != null, "Could not find column City");
        }

        [Test]
        public void StateProvinceId()
        {
            Assert.That(table.Columns["StateProvinceId"] != null, "Could not find column StateProvinceId");
        }

        [Test]
        public void PostalCode()
        {
            Assert.That(table.Columns["PostalCode"] != null, "Could not find column PostalCode");
        }

        [Test]
        public void SpatialLocation()
        {
            Assert.That(table.Columns["SpatialLocation"] != null, "Could not find column SpatialLocation");
        }

        [Test]
        public void RowGuid()
        {
            Assert.That(table.Columns["RowGuid"] != null, "Could not find column RowGuid");
        }

        [Test]
        public void ModifiedDate()
        {
            Assert.That(table.Columns["ModifiedDate"] != null, "Could not find column ModifiedDate");
        }

    }
}