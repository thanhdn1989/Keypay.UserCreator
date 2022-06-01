using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace UserCreator.Core
{
    public static class CoreRegistration
    {
        public static void RegisterCore(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IdentityManager>();
            serviceCollection.AddScoped<FileWriter>();
            serviceCollection.AddScoped<RecoveryService>();
            var rules = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && typeof(IParser).IsAssignableFrom(t))
                .ToList();
            if (rules.Count == 0) return;
            foreach (var rule in rules)
            {
                serviceCollection.AddScoped(rule);
            }
            serviceCollection.AddScoped<ParserService>();
            serviceCollection.AddScoped<ParserProvider>();
        }
    }
}