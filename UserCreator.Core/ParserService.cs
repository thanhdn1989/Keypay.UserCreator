using System;
using System.Collections.Generic;
using UserCreator.Core.Providers.Contracts;

namespace UserCreator.Core
{
    public static class ParserService
    {
        private static Dictionary<string, Func<string, string>> _parsers = new Dictionary<string, Func<string, string>>();

        public static void ParseFor(string fieldName, IParser parser)
        {
            _parsers.TryAdd(fieldName, parser.Parse);
        }

        public static void ParseFor(string fieldName, Func<string, string> parser)
        {
            _parsers.TryAdd(fieldName, parser);
        }

        public static bool TryParse(Field field, out string data)
        {
            data = field.Value;
            try
            {
                if (_parsers.ContainsKey(field.FieldName))
                    data = _parsers[field.FieldName].Invoke(field.Value);
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().ToString());
                Console.Out.WriteLine($"Could not convert {field.FieldName}!");
                return false;
            }
        }
    }
}