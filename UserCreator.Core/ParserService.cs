using UserCreator.Core.Contracts;

namespace UserCreator.Core
{
    public class ParserService
    {
        private readonly ParserProvider _parserProvider;

        public ParserService(ParserProvider parserProvider)
        {
            _parserProvider = parserProvider;
        }

        public bool TryParse(Field field, out string data)
        {
            data = field.Value;
            var parser = _parserProvider.GetParser(field.FieldName);
            if (parser == null) return true;
            if (!parser.TryParse(field.Value, out var result))
            {
                data = result.ToString();
                return false;
            }
            data = result.ToString();
            return true;

        }
    }
}