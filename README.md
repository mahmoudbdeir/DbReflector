# DbReflector
A class library that represents database objects, handy for code generation.

```csharp
using DbReflector;
using DbReflector.Common;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var cs = @"server=.\sqlexpress;database=AdventureWorksCS;Trusted_Connection=True;";
            ISchemaInfo ShemaInfo = SchemaInfoFactory.CreateInstance(cs, CSharpTableInfoFormatter.CreateInstance(), "myapp").Reflect();
            ITableInfo table = ShemaInfo.Tables["Person.Address"];

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
```
