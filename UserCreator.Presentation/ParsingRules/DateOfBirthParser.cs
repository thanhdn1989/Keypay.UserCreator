using System;
using System.Globalization;
using UserCreator.Core;
using UserCreator.Core.Constants;

namespace UserCreator.ParsingRules
{
    public class DateOfBirthParser : IParser
    {
        public string FieldName => FieldConstants.DateOfBirth;

        public string Parse(string fieldValue)
        {
            return DateTime.Parse(fieldValue).ToString(CultureInfo.InvariantCulture);
        }
    }
}