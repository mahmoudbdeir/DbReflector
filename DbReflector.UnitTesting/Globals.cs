using DbReflector.Common;

namespace DbReflector.UnitTesting
{

    public static class Globals
    {
        const string applicationName = "app2";
        const bool refreshMetadata = true;

        public static class AdventureWorksCs
        {
            public static ISchemaInfo ShemaInfo = SchemaInfoFactory.CreateInstance(Secrets.AdventureWorksCs, CSharpTableInfoFormatter.CreateInstance(), applicationName, refreshMetadata: refreshMetadata).Reflect();
        }
    }
}