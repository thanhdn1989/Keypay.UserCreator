using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserCreator.Core.Providers;
using UserCreator.Core.Providers.Contracts;

namespace UserCreator.Infrastructure.Providers
{
    /// <summary>
    /// Read input from console 
    /// </summary>
    public class ConsoleDatasourceProvider : IDatasourceProvider
    {
        public async IAsyncEnumerable<Field> ReadAsync()
        {
            string fieldType;
            while(!string.IsNullOrEmpty(fieldType = await GetFieldType()))
            {
                yield return new Field(fieldType, await GetData(fieldType));
                Console.WriteLine($"============");
            }
        }

        private static async Task<string> GetFieldType()
        {
            await Console.Out.WriteLineAsync($"Please enter field, or enter to exit");
            return await Console.In.ReadLineAsync();
        }

        private static async Task<string> GetData(string fieldName)
        {
            await Console.Out.WriteLineAsync($"Please enter user's {fieldName}:");
            return await Console.In.ReadLineAsync();
        }
    }
}