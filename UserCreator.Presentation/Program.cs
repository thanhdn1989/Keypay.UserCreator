using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UserCreator.Core;
using UserCreator.Infrastructure.Extensions;

namespace UserCreator
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static Mutex _mutex = new Mutex(true, "My App Name");
        static async Task<int> Main(string[] args)
        {
            if (!_mutex.WaitOne(0, false))
            {
                Console.WriteLine("Instance already running");
                return 0;
            }
            if(args.Length != 1)
            {
                await Console.Out.WriteLineAsync($"Usage: UserCreator [outputfile]");
                return 1;
            }

            RegisterServices();
            var recoveryService = _serviceProvider.GetService<RecoveryService>();
            var path = args[0];
            recoveryService!.TryRecoverLastSession(path);
            await using var fs = File.Open(path, FileMode.Append);
            await using var sw = new StreamWriter(fs);
            var fw = _serviceProvider.GetService<FileWriter>();
            await fw!.WriteAsync(sw);
            return 0;
        }

        private static void RegisterServices()
        {
            //setup our DI
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterInfrastructure();
            serviceCollection.RegisterCore();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
