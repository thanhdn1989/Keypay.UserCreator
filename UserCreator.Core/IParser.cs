namespace UserCreator.Core
{
    /// <summary>
    /// Parser contract
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Name of the field where the parsing method will be applied
        /// </summary>
        string FieldName { get; }
        bool TryParse(string fieldValue, out object result);
    }
}