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
            Assert.IsNotNull(table.Columns["AddressLine1"], "Could not find column AddressLine1");
        }

        [Test]
        public void AddressLine2()
        {
            Assert.IsNotNull(table.Columns["AddressLine2"], "Could not find column AddressLine2");
        }

        [Test]
        public void City()
        {
            Assert.IsNotNull(table.Columns["City"], "Could not find column City");
        }

        [Test]
        public void StateProvinceId()
        {
            Assert.IsNotNull(table.Columns["StateProvinceId"], "Could not find column StateProvinceId");
        }

        [Test]
        public void PostalCode()
        {
            Assert.IsNotNull(table.Columns["PostalCode"], "Could not find column PostalCode");
        }

        [Test]
        public void SpatialLocation()
        {
            Assert.IsNotNull(table.Columns["SpatialLocation"], "Could not find column SpatialLocation");
        }

        [Test]
        public void RowGuid()
        {
            Assert.IsNotNull(table.Columns["RowGuid"], "Could not find column RowGuid");
        }

        [Test]
        public void ModifiedDate()
        {
            Assert.IsNotNull(table.Columns["ModifiedDate"], "Could not find column ModifiedDate");
        }

    }
}