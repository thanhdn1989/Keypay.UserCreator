namespace UserCreator.Core.Providers.Contracts
{
    public class Field
    {
        public Field(string fieldName, string value)
        {
            Value = value;
            FieldName = fieldName;
        }

        public string FieldName { get; private set; }
        public string Value { get; private set; }
    }
}