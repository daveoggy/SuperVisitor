namespace SuperVisitor
{
    public interface IConfigVisitor<out TOut>
    {
        TOut Visit(SqlServerConfig sqlServerConfig);

        TOut Visit(CsvConfig csvConfig);
    }


}