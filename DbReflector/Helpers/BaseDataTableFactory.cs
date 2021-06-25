using System.Data;

namespace DbReflector.Helpers
{
    public abstract class BaseDataTableFactory
    {
        string connectionString;

        public class TablesAndViewsDataSet : DataSet
        {
            const string TABLES_AND_VIEWS = "TablesAndViews";
            const string TABLE_COLUMNS = "TableColumns";

            public TablesAndViewsDataSet(DataTable tablesAndViews, DataTable tableColumns)
            {
                tablesAndViews.TableName = TABLES_AND_VIEWS;
                Tables.Add(tablesAndViews);

                tableColumns.TableName = TABLE_COLUMNS;
                Tables.Add(tableColumns);
            }
            public DataTable TablesAndViews => Tables[TABLES_AND_VIEWS];
            public DataTable TableColumns => Tables[TABLE_COLUMNS];
        }

        public class ProcsDataSet: DataSet
        {
            const string PROCS = "Procs";
            const string PROC_PARAMS = "ProcParams";
            const string PROC_COLUMNS = "ProcColumns";

            public ProcsDataSet(DataTable procs, DataTable procParams, DataTable procColumns)
            {
                procs.TableName = PROCS;
                Tables.Add(procs);

                procParams.TableName = PROC_PARAMS;
                Tables.Add(procParams);

                procColumns.TableName = PROC_COLUMNS;
                Tables.Add(procColumns);
            }
            public DataTable Procs => Tables[PROCS];
            public DataTable ProcParams => Tables[PROC_PARAMS];
            public DataTable ProcColumns => Tables[PROC_COLUMNS];
        }

        public class UdtsDataSet : DataSet
        {
            const string UDTS = "Udts";
            const string UDT_COLUMNS = "UdtColumns";

            public UdtsDataSet(DataTable udts, DataTable udtColumns)
            {
                udts.TableName = UDTS;
                Tables.Add(udts);

                udtColumns.TableName = UDT_COLUMNS;
                Tables.Add(udtColumns);
            }
            public DataTable Udts => Tables[UDTS];
            public DataTable UdtColumns => Tables[UDT_COLUMNS];
        }


        public BaseDataTableFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public abstract TablesAndViewsDataSet TablesAndViews { get; }
        public abstract ProcsDataSet Procs { get; }
        public abstract UdtsDataSet Udts { get; }
    }
}