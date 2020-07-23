namespace SuperVisitor
{
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


}