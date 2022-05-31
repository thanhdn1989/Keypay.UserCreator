namespace UserCreator.Core
{
    /// <summary>
    /// Abstract class to define basic contract for a parser
    /// TODO: support advance parser
    /// </summary>
    public interface IParser
    {
        string FieldName { get; }
        string Parse(string fieldValue);
    }
}