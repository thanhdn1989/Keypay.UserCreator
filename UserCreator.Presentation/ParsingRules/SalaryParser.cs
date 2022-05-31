using System.Globalization;
using UserCreator.Core;
using UserCreator.Core.Constants;

namespace UserCreator.ParsingRules
{
    public class SalaryParser : IParser
    {
        public string FieldName => FieldConstants.Salary;

        public string Parse(string fieldValue)
        {
            return decimal.Parse(fieldValue).ToString(CultureInfo.InvariantCulture);
        }
    }
}