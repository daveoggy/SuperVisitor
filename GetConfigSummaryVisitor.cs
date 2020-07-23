namespace SuperVisitor
{
    public class GetConfigSummaryVisitor : IConfigVisitor<string>
    {
        public static GetConfigSummaryVisitor Instance { get; } = new GetConfigSummaryVisitor();

        private GetConfigSummaryVisitor()
        {

        }

        public string Visit(SqlServerConfig sqlServerConfig)
        {
            return $"Server=\"{sqlServerConfig.Server}\", Database=\"{sqlServerConfig.Server}\", Port={sqlServerConfig.Port}";
        }

        public string Visit(CsvConfig csvConfig)
        {
            return $"Filename=\"{csvConfig.Filename}\", Seperator=\"{csvConfig.Seperator}\"";
        }
    }


}