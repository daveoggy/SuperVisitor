using System.Threading.Tasks;

namespace SuperVisitor
{
    /// <summary>
    /// What's nice about using a delegate is that we can annotate it
    /// in a way that we simply can't do with <see cref="Action{T}"/> or <see cref="Func{T, TResult}"/>
    /// </summary>
    /// <param name="runtimeData">This is an extra argument that the visitor needs to do it's work</param>
    /// <returns>A string which describes the visited type</returns>
    public delegate Task<string> GetDescriptionUsingRuntimeData(RuntimeData runtimeData);

    public class UseADelegateVisitor : IConfigVisitor<GetDescriptionUsingRuntimeData>
    {
        public static UseADelegateVisitor Instance { get; } = new UseADelegateVisitor();
        
        private UseADelegateVisitor()
        {

        }

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