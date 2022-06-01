using System;
using System.Globalization;
using UserCreator.Core;
using UserCreator.Core.Constants;

namespace UserCreator.ParsingRules
{
    public class DateOfBirthParser : IParser
    {
        public string FieldName => FieldConstants.DateOfBirth;

        public bool TryParse(string fieldValue, out object result)
        {
            try
            {
                result = DateTime.Parse(fieldValue);
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().ToString());
                Console.Out.WriteLine($"Could not convert {fieldValue} to {nameof(DateTime)}!");
                result = default(DateTime);
                return false;
            }
        }
    }
}