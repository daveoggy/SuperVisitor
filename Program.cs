using System;
using System.Threading.Tasks;

namespace SuperVisitor
{
    internal static class Program
    {
        private static readonly GetConfigSummaryVisitor _getConfigSummaryVisitor = new GetConfigSummaryVisitor();
        private static readonly UseRuntimeDataInSummaryVisitor _useRuntimeDataInSummary = new UseRuntimeDataInSummaryVisitor();
        private static readonly CombineVisitors _combineVisitors = new CombineVisitors();
        private static readonly AsyncVisitor _asyncVisitor = new AsyncVisitor();
        private static readonly UseADelegateVisitor _delegateTest = new UseADelegateVisitor();

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

            var sqlSummary = sqlServer.Accept(_getConfigSummaryVisitor);
            var csvSummary = csvFile.Accept(_getConfigSummaryVisitor);

            Console.WriteLine(sqlSummary);
            Console.WriteLine(csvSummary);

            var runtimeData = new RuntimeData();

            var runtimeSummary = sqlServer.Accept(_useRuntimeDataInSummary)(runtimeData);

            Console.WriteLine(runtimeSummary);

            var combinedSummarySql = sqlServer.Accept(_combineVisitors)(runtimeData, _getConfigSummaryVisitor);
            var combinedSummaryCSV = csvFile.Accept(_combineVisitors)(runtimeData, _getConfigSummaryVisitor);

            Console.WriteLine(combinedSummarySql);
            Console.WriteLine(combinedSummaryCSV);

            var asyncSqlString = await sqlServer.Accept(_asyncVisitor);
            var asyncCsvString = await csvFile.Accept(_asyncVisitor);

            Console.WriteLine(asyncSqlString);
            Console.WriteLine(asyncCsvString);

            var t1 = await sqlServer.Accept(_delegateTest)(runtimeData);
            var t2 = await csvFile.Accept(_delegateTest)(runtimeData);

            Console.WriteLine(t1);
            Console.WriteLine(t2);

            GetDescriptionUsingRuntimeData y = sqlServer.Accept(_delegateTest);

            await y.Invoke(runtimeData);

            return 0;
        }
    }

    public class RuntimeData
    {
        public Guid Value { get; } = Guid.NewGuid();
    }

    public class SqlServerConfig : IConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }

        public int Port { get; set; }

        public TOut Accept<TOut>(IConfigVisitor<TOut> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class CsvConfig : IConfig
    {
        public string Filename { get; set; }
        public char Seperator { get; set; }

        public TOut Accept<TOut>(IConfigVisitor<TOut> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public interface IConfigVisitor<out TOut>
    {
        TOut Visit(SqlServerConfig sqlServerConfig);

        TOut Visit(CsvConfig csvConfig);
    }

    public interface IConfig
    {
        TOut Accept<TOut>(IConfigVisitor<TOut> visitor);
    }

    public class GetConfigSummaryVisitor : IConfigVisitor<string>
    {
        public string Visit(SqlServerConfig sqlServerConfig)
        {
            return $"Server=\"{sqlServerConfig.Server}\", Database=\"{sqlServerConfig.Server}\", Port={sqlServerConfig.Port}";
        }

        public string Visit(CsvConfig csvConfig)
        {
            return $"Filename=\"{csvConfig.Filename}\", Seperator=\"{csvConfig.Seperator}\"";
        }
    }

    public class UseRuntimeDataInSummaryVisitor : IConfigVisitor<Func<RuntimeData, string>>
    {
        public Func<RuntimeData, string> Visit(SqlServerConfig sqlServerConfig)
        {
            return (runtimeData) => AppendRuntimeData(sqlServerConfig, runtimeData);
        }

        public Func<RuntimeData, string> Visit(CsvConfig csvConfig)
        {
            return (runtimeData) => AppendRuntimeData(csvConfig, runtimeData);
        }

        private string AppendRuntimeData(IConfig config, RuntimeData runtimeData)
        {
            return runtimeData.Value.ToString();
        }
    }

    public class CombineVisitors : IConfigVisitor<Func<RuntimeData, GetConfigSummaryVisitor, string>>
    {
        public Func<RuntimeData, GetConfigSummaryVisitor, string> Visit(SqlServerConfig sqlServerConfig)
        {
            return (runtimeData, getConfigSummary) => sqlServerConfig.Accept(getConfigSummary) + $", RuntimeData={runtimeData.Value}";
        }

        public Func<RuntimeData, GetConfigSummaryVisitor, string> Visit(CsvConfig csvConfig)
        {
            return (runtimeData, getConfigSummary) => csvConfig.Accept(getConfigSummary) + $", RuntimeData={runtimeData.Value}";
        }
    }

    public class AsyncVisitor : IConfigVisitor<Task<string>>
    {
        public async Task<string> Visit(SqlServerConfig sqlServerConfig)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return "I waited 1 second";
        }

        public async Task<string> Visit(CsvConfig csvConfig)
        {
            await Task.Delay(2000).ConfigureAwait(false);
            return "I waited 2 second";
        }
    }

    /// <summary>
    /// What's nice about using a delegate is that we can annotate it
    /// in a way that we simply can't do with <see cref="Action{T}"/> or <see cref="Func{T, TResult}"/>
    /// </summary>
    /// <param name="runtimeData">This is an extra argument that the visitor needs to do it's work</param>
    /// <returns>A string which describes the visited type</returns>
    public delegate Task<string> GetDescriptionUsingRuntimeData(RuntimeData runtimeData);

    public class UseADelegateVisitor : IConfigVisitor<GetDescriptionUsingRuntimeData>
    {
        public GetDescriptionUsingRuntimeData Visit(SqlServerConfig sqlServerConfig)
        {
            return async (runtimeData) =>
            {
                await Task.Delay(1000).ConfigureAwait(false);
                return runtimeData.Value.ToString();
            };
        }

        public GetDescriptionUsingRuntimeData Visit(CsvConfig csvConfig)
        {
            return async (runtimeData) =>
            {
                await Task.Delay(1000).ConfigureAwait(false);
                return runtimeData.Value.ToString();
            };
        }
    }
}