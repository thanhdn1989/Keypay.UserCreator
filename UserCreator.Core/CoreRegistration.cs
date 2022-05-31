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
            RegisterParserRule();
        }

        public static void RegisterParserRule()
        {
            var rules = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && typeof(IParser).IsAssignableFrom(t))
                .ToList();
            if (rules.Count == 0) return;
            foreach (var instance in rules.Select(rule => (IParser)Activator.CreateInstance(rule)).Where(instance => instance != null))
            {
                ParserService.ParseFor(instance.FieldName, instance.Parse);
            }
        }

    }
}