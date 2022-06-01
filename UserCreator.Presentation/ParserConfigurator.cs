using Microsoft.Extensions.DependencyInjection;
using UserCreator.Core;
using UserCreator.ParsingRules;

namespace UserCreator
{
    public static class ParserConfigurator
    {
        public static void RegisterParsers(this IServiceCollection serviceCollection)
        {
            // serviceCollection.AddScoped<DateOfBirthParser>();
            // serviceCollection.AddScoped<SalaryParser>();
            
        }
    }
}