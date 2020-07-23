using System;

namespace SuperVisitor
{
    public class CombineVisitors : IConfigVisitor<Func<RuntimeData, GetConfigSummaryVisitor, string>>
    {
        public static CombineVisitors Instance { get; } = new CombineVisitors();

        private CombineVisitors()
        {

        }

        public Func<RuntimeData, GetConfigSummaryVisitor, string> Visit(SqlServerConfig sqlServerConfig)
        {
            return (runtimeData, getConfigSummary) => sqlServerConfig.Accept(getConfigSummary) + $", RuntimeData={runtimeData.Value}";
        }

        public Func<RuntimeData, GetConfigSummaryVisitor, string> Visit(CsvConfig csvConfig)
        {
            return (runtimeData, getConfigSummary) => csvConfig.Accept(getConfigSummary) + $", RuntimeData={runtimeData.Value}";
        }
    }


}