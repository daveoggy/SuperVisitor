namespace SuperVisitor
{
    public class CsvConfig : IConfig
    {
        public string Filename { get; set; }
        public char Seperator { get; set; }

        public TOut Accept<TOut>(IConfigVisitor<TOut> visitor)
        {
            return visitor.Visit(this);
        }
    }


}