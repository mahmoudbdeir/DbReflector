namespace DbReflector.Common
{
    public class TableInfoFormatter : ITableInfoFormatterDecorator
    {
        ITableInfoFormatter genericFormatter;
        ITableInfo tableInfo;
        public TableInfoFormatter(ITableInfo tableInfo, ITableInfoFormatter genericFormatter)
        {
            this.genericFormatter = genericFormatter;
            this.tableInfo = tableInfo;
        }
        public string CsvColumns => genericFormatter.CsvColumns(tableInfo);
        public string CsvColumnsWithAt => genericFormatter.CsvColumnsWithAt(tableInfo);
        public string CsvColumnsWithAtAndType => genericFormatter.CsvColumnsWithAtAndSqlType(tableInfo);

        public string CsvColumnsNoPk => genericFormatter.CsvColumnsNoPk(tableInfo);

        public string CsvColumnsNoPkWithAt => genericFormatter.CsvColumnsNoPkWithAt(tableInfo);

        public string CsvColumnsWithAtAndSqlType => genericFormatter.CsvColumnsWithAtAndSqlType(tableInfo);

        public string CsvColumnsNoPkWithAtAndSqlType => genericFormatter.CsvColumnsNoPkWithAtAndSqlType(tableInfo);

        public string CsvColumnsWithAtAndAlias => genericFormatter.CsvColumnsWithAtAndAlias(tableInfo);

        public string CSharpFuncParams => genericFormatter.CSharpFuncParams(tableInfo);

        public string CSharpEntityNameCamelCase => genericFormatter.CSharpEntityNameCamelCase(tableInfo);

        public string CSharpFuncInvokeParams => genericFormatter.CSharpFuncInvokeParams(tableInfo);
    }
}