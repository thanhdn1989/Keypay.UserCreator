using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UserCreator.Core;
using UserCreator.Core.Providers;
using UserCreator.Extensions;
using UserCreator.Infrastructure.Extensions;
using UserCreator.Infrastructure.Providers;
using UserCreator.ParsingRules;

namespace UserCreator
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static async Task<int> Main(string[] args)
        {
            // if(args.Length != 1)
            // {
            //     await Console.Out.WriteLineAsync($"Usage: UserCreator [outputfile]");
            //     return 1;
            // }

            RegisterServices();
            var recoveryService = _serviceProvider.GetService<RecoveryService>();
            // var path = args[0] != null ? args[0] : @"E:\Users.txt";
            var path = @"E:\Users.txt";
            recoveryService!.TryRecoverLastSession(path);
            await using var fs = File.Open(path, FileMode.Append);
            await using var sw = new StreamWriter(fs);
            using var fw = _serviceProvider.GetService<FileWriter>();
            await fw!.WriteAsync(sw);
            return 0;
        }

        private static void RegisterServices()
        {
            //setup our DI
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterInfrastructure();
            serviceCollection.RegisterParser();
            serviceCollection.RegisterCore();
            // serviceCollection.RegisterParserRule();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
