using System.Threading.Tasks;

namespace SuperVisitor
{
    public class AsyncVisitor : IConfigVisitor<Task<string>>
    {
        public static AsyncVisitor Instance { get; } = new AsyncVisitor();

        private AsyncVisitor()
        {

        }

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


}