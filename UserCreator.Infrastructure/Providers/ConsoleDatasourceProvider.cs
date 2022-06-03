using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserCreator.Core.Contracts;
using UserCreator.Core.Providers;

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
                var data = await GetData(fieldType);
                if (string.IsNullOrEmpty(data)) continue;
                yield return new Field(fieldType.Trim(), data.Trim());
                Console.WriteLine("============");
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