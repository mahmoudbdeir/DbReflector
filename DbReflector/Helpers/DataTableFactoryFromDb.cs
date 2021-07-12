using DbReflector.Helpers;
using System.Diagnostics;

namespace DbReflector
{
    public class DataTableFactoryFromDb : BaseDataTableFactory
    {
        SqlHelper _sqlHelper;
        public DataTableFactoryFromDb(string connectionString, string applicationName, bool refreshMetadata = false) : base(connectionString)
        {
            _sqlHelper = SqlHelper.CreateInstance(connectionString, applicationName, refreshMetadata);

            var sw = new Stopwatch();
            System.Console.Write("retrieving model from 6 select statements..");
            sw.Start();
            TablesAndViews = new TablesAndViewsDataSet(
                _sqlHelper.GetDataTable($"select * from TablesAndViews where application_name = '{applicationName}'", "TablesAndViews")
                ,_sqlHelper.GetDataTable($"select * from TableColumns where application_name = '{applicationName}'", "TableColumns")
            );

            Procs = new ProcsDataSet(
                _sqlHelper.GetDataTable($"select * from  Procs where application_name = '{applicationName}'", "Procs")
                ,_sqlHelper.GetDataTable($"select * from ProcParams where application_name = '{applicationName}'", "ProcParams")
                ,_sqlHelper.GetDataTable($"select * from ProcColumns where application_name = '{applicationName}'", "ProcColumns")

            );

            Udts = new UdtsDataSet(_sqlHelper.GetDataTable($"select * from Udts where application_name = '{applicationName}'", "Udts"),
                _sqlHelper.GetDataTable($"select * from UdtColumns where application_name = '{applicationName}'", "UdtColumns")
            );
            sw.Stop();
            System.Console.WriteLine($"{sw.Elapsed.Seconds} seconds.");
        }


        public override TablesAndViewsDataSet TablesAndViews { get; }
            

        public override ProcsDataSet Procs { get; }



        public override UdtsDataSet Udts { get; }
                
    }
}