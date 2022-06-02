using System;
using System.Collections.Generic;
using System.Linq;

namespace UserCreator.Core
{
    public class ParserProvider
    {
        private static readonly Dictionary<string, Type> Parsers = 
            new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        private static readonly object Lock = new object();

        private readonly IServiceProvider _serviceProvider;
        
        public ParserProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Bootstrap();
        }

        /// <summary>
        /// Get a parser base of its field name
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public IParser GetParser(string fieldName)
        {
            if (Parsers.ContainsKey(fieldName))
                return (IParser)_serviceProvider.GetService(Parsers[fieldName]);
            return null;
        }

        /// <summary>
        /// Use for manually add a Parser
        /// </summary>
        /// <param name="fieldName"></param>
        public static void AddParser<T>(string fieldName) where T : IParser
        {
            if (!Parsers.ContainsKey(fieldName))
                Parsers.Add(fieldName, typeof(T));
        }

        /// <summary>
        /// We will only add Parser which has been registered in IoC container to current dictionary
        /// Since Parser can have external dependency so we need to make sure that it's resolved correctly
        /// </summary>
        private void Bootstrap()
        {
            if (Parsers.Count > 0) return;
            // Prevent possible race condition 
            lock (Lock)
            {
                var rules = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && typeof(IParser).IsAssignableFrom(t))
                    .ToList();

                foreach (var rule in rules)
                {
                    var registerService = _serviceProvider.GetService(rule);
                    if (registerService != null)
                    {
                        Parsers.Add(((IParser)registerService).FieldName, rule);
                    }
                }
            }
        }
    }
}