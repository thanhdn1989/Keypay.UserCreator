using System;
using System.Globalization;
using UserCreator.Core;
using UserCreator.Core.Constants;

namespace UserCreator.ParsingRules
{
    public class SalaryParser : IParser
    {
        public string FieldName => FieldConstants.Salary;
        public bool TryParse(string fieldValue, out object result)
        {
            try
            {
                result = decimal.Parse(fieldValue);
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().ToString());
                Console.Out.WriteLine($"Could not convert {fieldValue} to {nameof(Decimal)}!");
                result = default(decimal);
                return false;
            }
        }

        public decimal Parse(string fieldValue)
        {
            try
            {
                return decimal.Parse(fieldValue);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}