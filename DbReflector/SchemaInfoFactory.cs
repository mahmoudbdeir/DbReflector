using DbReflector.Common;
using DbReflector.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbReflector
{
    public class SchemaInfoFactory
    {
        string _connectionString;
        string _applicationName;
        bool _refreshMetadata;

        ISchemaInfo _schemaInfo;
        ITableInfoFormatter _formatter;
        SchemaInfoFactory(string connectionString, ITableInfoFormatter formatter, string applicationName, bool refreshMetadata = true)
        {
            this._connectionString = connectionString;
            this._applicationName = applicationName;
            this._refreshMetadata = refreshMetadata;
            this._formatter = formatter;
            _schemaInfo = new SchemaInfo(connectionString, formatter);
        }
        static Dictionary<int, SchemaInfoFactory> _schemaInfoFactory = new Dictionary<int, SchemaInfoFactory>();
        static readonly char[] ForbiddenApplicationNameCharacters = new char[] { ' ' };
        private static object o = new object();
        public static SchemaInfoFactory CreateInstance(string connectionString, ITableInfoFormatter formatter, string applicationName, bool refreshMetadata = true)
        {
            if (string.IsNullOrWhiteSpace(applicationName) || applicationName.IndexOfAny(ForbiddenApplicationNameCharacters) > 0)
                throw new Exception("applicationName cannot be null or empty string nor can it contain spaces.");

            lock (o)
            {
                var hash = connectionString.GetHashCode();
                if (refreshMetadata || _schemaInfoFactory.ContainsKey(hash) == false)
                {
                    if (_schemaInfoFactory.ContainsKey(hash)) _schemaInfoFactory.Remove(hash);
                    _schemaInfoFactory.Add(hash, new SchemaInfoFactory(connectionString, formatter, applicationName, refreshMetadata));
                }
                return _schemaInfoFactory[hash];
            }
        }
        public ISchemaInfo Reflect()
        {

            try
            {
                BaseDataTableFactory factory = new DataTableFactoryFromDb(_connectionString, _applicationName, _refreshMetadata);

                DbTableReflector.CreateInstance().Reflect(factory, _schemaInfo);
                DbProcReflector.CreateInstance().Reflect(factory, _schemaInfo);
                DbUdtReflector.CreateInstance().Reflect(factory, _schemaInfo);
                return _schemaInfo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}