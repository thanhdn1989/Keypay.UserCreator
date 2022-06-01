namespace UserCreator.Core.Contracts
{
    public class Field
    {
        public Field(string fieldName, string value)
        {
            Value = value;
            FieldName = fieldName;
        }

        public string FieldName { get; }
        public string Value { get; }
    }
}