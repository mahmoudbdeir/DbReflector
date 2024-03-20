using DbReflector.Common;
using DbReflector.Helpers;

namespace DbReflector
{
    interface IBaseReflector
    {
        ISchemaInfo Reflect(BaseDataTableFactory factory, ISchemaInfo schemaInfo);
    }
}
