using DbReflector;
using DbReflector.Common;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var cs = @"server=.\sqlexpress;database=EntityMakerUnitTesting;Trusted_Connection=True;";
            var cs = @"server=.\sqlexpress;database=AdventureWorks2019;Trusted_Connection=True;";
            ISchemaInfo SchemaInfo = SchemaInfoFactory.CreateInstance(cs, CSharpTableInfoFormatter.CreateInstance(), "myapp", refreshMetadata: false).Reflect();
            ITableInfo table = SchemaInfo.Tables["Person.Person"];

            Console.WriteLine(table.TableFQN);
            Console.WriteLine("\nAll columns");
            foreach (var col in table.Columns.Values)
            {
                string dbtype = $"{col.DataType}" + (col.MaxLength==null? string.Empty : $"({col.MaxLength})");
                Console.WriteLine($"\n\nColumn: {col.ColumnName}");
                Console.WriteLine($"Domain: {col.DomainName}");
                Console.WriteLine($"Db Type: {dbtype}");
                Console.WriteLine($"C# Type: {col.CSharpType}");
                Console.WriteLine($"Primary Key? {col.IsPrimaryKey}");
                Console.WriteLine($"Foreign Key? {col.IsForeignKey}");
                Console.WriteLine($"Nullable? {col.IsNullable}");
                
            }
        }
    }
}
