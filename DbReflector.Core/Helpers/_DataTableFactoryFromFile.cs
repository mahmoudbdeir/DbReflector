//! in the future this will be used to cache the configuration tables (DbReflector.ProcColumns, etc.)
//using DbReflector.Helpers;
//using Microsoft.Data.SqlClient;
//using System.Data;
//using System.IO;

//namespace DbReflector
//{
//    public class DataTableFactoryFromFile : BaseDataTableFactory
//    {
//        public DataTableFactoryFromFile(string connectionString) : base(connectionString){}

//        /// <summary>
//        /// Gets the data set either from the local cache or from the database.
//        /// </summary>
//        /// <param name="funcName">The name of the function for which you are passing the SQL statement. Examples: AddTablesAndViews, AddUdts,...</param>
//        /// <param name="sql">The SQL statement for which to retrieve a data set.</param>
//        /// <param name="useCache">If true then gets the data set from cache if it exists.</param>
//        /// <param name="ConnectionString">The database connection string.</param>
//        /// <returns>A DataTable that contains the rows produced by running the sql statement.</returns>
//        public override DataTable GetDataSet(FunctionName funcName, bool useCache)
//        {
            
//            var CacheFile = Path.Combine(Path.GetTempPath(), ConnectionString.GetHashCode() + funcName + ".xml");
//            DataSet dataset = new DataSet(funcName.ToString());
//            if (!useCache || !File.Exists(CacheFile))
//            {
//                using (var cn = new SqlConnection(ConnectionString))
//                {
//                    cn.Open();
//                    using (var cmd = new SqlCommand(sql, cn))
//                    {
//                        new SqlDataAdapter(cmd).Fill(dataset);
//                        dataset.WriteXml(CacheFile);
//                    }
//                }
//            }
//            dataset = new DataSet(funcName.ToString());
//            FileStream streamRead = new FileStream(CacheFile,FileMode.Open);
//            dataset.ReadXml(streamRead);
//            streamRead.Close();
//            streamRead.Dispose();
//            return dataset.Tables[0];
//        }
//    }
//}
