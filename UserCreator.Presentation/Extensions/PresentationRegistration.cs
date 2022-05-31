using Microsoft.Extensions.DependencyInjection;
using UserCreator.ParsingRules;

namespace UserCreator.Extensions
{
    public static class PresentationRegistration
    {
        public static void RegisterParser(this IServiceCollection serviceCollection)
        {
            // serviceCollection
            //     .AddScoped<DateTimeParser>()
            //     .AddScoped<DecimalParser>();

        }
    }
}