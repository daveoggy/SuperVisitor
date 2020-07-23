using System;

namespace SuperVisitor
{
    public class UseRuntimeDataInSummaryVisitor : IConfigVisitor<Func<RuntimeData, string>>
    {
        public static UseRuntimeDataInSummaryVisitor Instance { get; } = new UseRuntimeDataInSummaryVisitor();

        private UseRuntimeDataInSummaryVisitor()
        {

        }

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


}