using DbReflector;
using DbReflector.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        const string procTemplate =
@"
using BRG.Helen.Backend.Core;
using Dapper;
using System;
using System.Data.SqlClient;
namespace BRG.Helen.Backend.Alexa.DAL;

public partial class Procs
{
    public static {{FunctionReturnParams}} {{functionName}}({{functionParams}})
    {
        try
        {
            using (var cn = new SqlConnection(Globals.ConnectionString))
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add(""@returnValue"", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);
                {{params}}
                cn.Open();
                cn.Execute(""{{procName}}"", dynamicParameters, commandType: System.Data.CommandType.StoredProcedure);

                int procReturnValue = dynamicParameters.Get<int>(""@returnValue"");
                {{outputParamValues}}
                {{return}}
            }
        }
        catch (Exception ex)
        {
            return {{error}};
        }
    }
}
";
        const string OutputPath = @"C:\Users\Mahmoud Bdeir\source\repos\BRG.Helen.Backend.Alexa\DAL";
        static void Main(string[] args)
        {

            var cs = Environment.GetEnvironmentVariable("connectionString");
            ISchemaInfo SchemaInfo = SchemaInfoFactory.CreateInstance(cs, CSharpTableInfoFormatter.CreateInstance(), "myapp", refreshMetadata: false).Reflect();
            const string procName = "tools.spCreateConcierge";
            IProcInfo proc = SchemaInfo.Procedures[procName];
            WriteProcBody(proc);
            //GenerateProc(proc);
        }
        readonly static string[] StringTypes = new string[3] { "char", "varchar", "nvarchar" };
        readonly static string[] NumberTypes = new string[1] { "int"};
        
        static void WriteProcBody(IProcInfo proc)
        {
            File.WriteAllText($@"{OutputPath}\{proc.ProcName}.cs", proc.SourceCode);
        }
        private static void GenerateProc(IProcInfo proc)
        {
            
            StringBuilder sb = new();
            StringBuilder ProcOutputParams = new();
            List<string> FunctionReturnParams = new();
            FunctionReturnParams.Add("Exception exception");
            FunctionReturnParams.Add("int? procReturnValue");
            List<string> ReturnResult = new();
            List<string> ReturnError = new();
            List<string> FunctionParams = new();

            foreach (var p in proc.Parameters)
            {
                //string type = string.IsNullOrWhiteSpace(p.Value.ParameterUDTName) ? p.Value.ParameterType : p.Value.ParameterUDTName;
                //if (p.Value.ParameterMaxLen > 0 && string.IsNullOrWhiteSpace(p.Value.ParameterUDTName)) type += $"({p.Value.ParameterMaxLen})";
                string ProcParamName = p.Key;
                string ProcParamType = p.Value.ParameterType switch
                {
                    var x when StringTypes.Contains(x) => $"System.Data.DbType.String, size: {p.Value.ParameterMaxLen}",
                    var x when NumberTypes.Contains(x) => $"System.Data.DbType.Int32",
                    "bit" => $"System.Data.DbType.Boolean",
                    "datetime" => "System.Data.DbType.DateTime",
                    _ => $"DON'T KNOW {p.Value.ParameterType}",
                };
                string FunctionParamType = p.Value.ParameterType switch
                {
                    var x when StringTypes.Contains(x) => $"string",
                    "bit" => $"bool",
                    "datetime" => "DateTime",
                    var x when NumberTypes.Contains(x) => $"int",
                    _ => $"DON'T KNOW {p.Value.ParameterType}"
                };
                object FunctionParamTypeDefaultValue = p.Value.ParameterType switch
                {
                    var x when StringTypes.Contains(x) => $"string.Empty",
                    _ => $"default({FunctionParamType})",
                };
                string ProcParamDirection = p.Value.Direction switch
                {
                    DirectionEnum.Out => "System.Data.ParameterDirection.Output",
                    DirectionEnum.In => "System.Data.ParameterDirection.Input",
                    DirectionEnum.Unknown => "unknown",
                };
                string FunctionParamName = $"{p.Key.Replace("@", "")}";
                string ProcParamValue = p.Value.Direction switch
                {
                    DirectionEnum.In => $",value: {FunctionParamName}",
                    _ => string.Empty,
                };
                string LocalParam = $"{FunctionParamType} p{p.Key.Replace("@", "")}";

                if (p.Value.Direction == DirectionEnum.In)
                {
                    FunctionParams.Add($"{FunctionParamType} {FunctionParamName}");
                }

                if (p.Value.Direction == DirectionEnum.Out)
                {
                    FunctionReturnParams.Add($"{FunctionParamType} {FunctionParamName}");
                    ProcOutputParams.AppendLine($"{FunctionParamType}? {FunctionParamName} = dynamicParameters.Get<{FunctionParamType}?>(\"{p.Key}\");");
                    ProcOutputParams.AppendLine($"{FunctionParamName} = {FunctionParamName} ?? {FunctionParamTypeDefaultValue};");

                    ReturnResult.Add($"({FunctionParamType}){FunctionParamName}");
                    ReturnError.Add($"{FunctionParamTypeDefaultValue}");
                }
                sb.AppendLine($"dynamicParameters.Add(\"{ProcParamName}\", dbType: {ProcParamType}, direction: {ProcParamDirection} {ProcParamValue});");
            }
            string code = procTemplate
                .Replace("{{procName}}", $"{proc.ProcSchema}.{proc.ProcName}")
                .Replace("{{functionName}}", proc.ProcName.Replace(".", "_"))
                .Replace("{{params}}", sb.ToString())
                .Replace("{{outputParamValues}}", ProcOutputParams.ToString())
                .Replace("{{FunctionReturnParams}}", $"({string.Join(",", FunctionReturnParams)})")
                .Replace("{{return}}", $"return (null, procReturnValue, {string.Join(",", ReturnResult)});")
                .Replace("{{error}}", $"(ex,null,{string.Join(",", ReturnError)})")
                .Replace("{{functionParams}}", $"{string.Join(",", FunctionParams)}")
                ;
            string file = $@"{OutputPath}\{proc.ProcName}.cs";
            File.WriteAllText(file, code);
            Console.WriteLine(file);
        }
    }
}