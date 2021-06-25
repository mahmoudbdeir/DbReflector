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
            var cs = @"server=.\sqlexpress;database=12;Trusted_Connection=True;";
            ISchemaInfo ShemaInfo = SchemaInfoFactory.CreateInstance(cs, CSharpTableInfoFormatter.CreateInstance(), "myapp").Reflect();
            ITableInfo table = ShemaInfo.Tables["schema.table"];
            
            Console.WriteLine("\nAll columns");
            foreach (var col in table.Columns)
            {
                Console.WriteLine(col.Key);
            }

            Console.WriteLine("\nColumns that are not primary keys");
            foreach (var col in table.Columns_NotPrimaryKeys)
            {
                Console.WriteLine(col.ColumnKey);
            }

            Console.WriteLine("\nColumns that do not allow nulls");
            foreach (var col in table.Columns_NotNullable)
            {
                Console.WriteLine(col.ColumnKey);
            }

            Console.WriteLine("\nColumns that are foreign keys");
            foreach (var col in table.Columns_ForeignKeys)
            {
                Console.WriteLine(col.ColumnKey);
            }

        }
    }
}

```
