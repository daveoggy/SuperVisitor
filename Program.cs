using System;
using System.Threading.Tasks;

namespace SuperVisitor
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            SuperVisitor.Potato.Potato.DoStuff();
            Console.WriteLine("Hello World!");

            var sqlServer = new SqlServerConfig
            {
                Server = "cda-dev-db02.devid.local",
                Database = "CDA-DEV",
                Port = 1512
            };

            var csvFile = new CsvConfig
            {
                Filename = @"C:\temp\test.csv",
                Seperator = '|'
            };

            var sqlSummary = sqlServer.Accept(GetConfigSummaryVisitor.Instance);
            var csvSummary = csvFile.Accept(GetConfigSummaryVisitor.Instance);

            Console.WriteLine(sqlSummary);
            Console.WriteLine(csvSummary);

            var runtimeData = new RuntimeData();
            var runtimeData2 = new RuntimeData();

            var runtimeSummary = sqlServer.Accept(UseRuntimeDataInSummaryVisitor.Instance)(runtimeData);

            Console.WriteLine(runtimeSummary);

            var combinedSummarySql = sqlServer.Accept(CombineVisitors.Instance)(runtimeData, GetConfigSummaryVisitor.Instance);
            var combinedSummaryCSV = csvFile.Accept(CombineVisitors.Instance)(runtimeData, GetConfigSummaryVisitor.Instance);

            Console.WriteLine(combinedSummarySql);
            Console.WriteLine(combinedSummaryCSV);

            var asyncSqlString = await sqlServer.Accept(AsyncVisitor.Instance);
            var asyncCsvString = await csvFile.Accept(AsyncVisitor.Instance);

            Console.WriteLine(asyncSqlString);
            Console.WriteLine(asyncCsvString);

            var t1 = await sqlServer.Accept(UseADelegateVisitor.Instance)(runtimeData);
            var t2 = await csvFile.Accept(UseADelegateVisitor.Instance)(runtimeData2);

            Console.WriteLine(t1);
            Console.WriteLine(t2);

            GetDescriptionUsingRuntimeData y = sqlServer.Accept(UseADelegateVisitor.Instance);

            await y.Invoke(runtimeData);

            return 0;
        }
    }


}