namespace DbReflector.Common
{

    public interface ITableInfoFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>MaterialID, ActionID, Cost, Manufacturer, PartDescription, PartNumber, Vendor</returns>
        string CsvColumns(ITableInfo tableInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>"@ActionID, @Cost, @Manufacturer, @PartDescription, @PartNumber, @Vendor"</returns>
        string CsvColumnsWithAt(ITableInfo tableInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>@ActionID INT, @Cost money, @Manufacturer varchar(100), @PartDescription varchar(100), @PartNumber varchar(25), @Vendor varchar(50)</returns>
        string CsvColumnsWithAtAndSqlType(ITableInfo tableInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        string CSharpFuncParams(ITableInfo tableInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>"ActionID, Cost, Manufacturer, PartDescription, PartNumber, Vendor";</returns>
        string CsvColumnsNoPk(ITableInfo tableInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>@ActionID, @Cost, @Manufacturer, @PartDescription, @PartNumber, @Vendor</returns>

        string CsvColumnsNoPkWithAt(ITableInfo tableInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns>@ActionID INT, @Cost money, @Manufacturer varchar(100), @PartDescription varchar(100), @PartNumber varchar(25), @Vendor varchar(50)</returns>

        string CsvColumnsNoPkWithAtAndSqlType(ITableInfo tableInfo);

        string CsvColumnsWithAtAndAlias(ITableInfo tableInfo);

        string CSharpEntityNameCamelCase(ITableInfo tableInfo);

        string CSharpFuncInvokeParams(ITableInfo tableInfo);
    }
}