using System;

namespace SuperVisitor.Potato
{
    public static class Potato
    {
        public static void DoStuff()
        {
            var sqlServer = new SqlServerConfig
            {
                Server = "cda-dev-db02.devid.local",
                Database = "CDA-DEV",
                Port = 1512
            };

            var visitor = new GetConfigSummaryVisitor();

            sqlServer.Accept(visitor);

            Console.WriteLine(visitor.Result);
        }
    }

    public interface IConfig
    {
        void Accept(IConfigVisitor visitor);
    }

    public interface IConfigVisitor
    {
        void Visit(SqlServerConfig sqlServerConfig);
        void Visit(OracleConfig oracleConfig);
    }

    public class GetConfigSummaryVisitor : IConfigVisitor
    {
        public string Result { get; set; }

        public void Visit(SqlServerConfig sqlServerConfig)
        {
            Result = $"Server=\"{sqlServerConfig.Server}\", Database=\"{sqlServerConfig.Server}\", Port={sqlServerConfig.Port}";
        }

        public void Visit(OracleConfig csvConfig)
        {
            Result = $"Filename=\"{csvConfig.Database}\", Seperator=\"{csvConfig.Server}\"";
        }
    }

    public class SqlServerConfig : IConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public int Port { get; set; }

        public void Accept(IConfigVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class OracleConfig : IConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public int Port { get; set; }

        public void Accept(IConfigVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
