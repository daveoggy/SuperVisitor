namespace SuperVisitor
{
    public interface IConfig
    {
        TOut Accept<TOut>(IConfigVisitor<TOut> visitor);
    }


}