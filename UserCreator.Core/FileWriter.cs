using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserCreator.Core.Contracts;
using UserCreator.Core.Providers;

namespace UserCreator.Core
{
    public class FileWriter : IDisposable
    {
        private readonly IDatasourceProvider _datasourceProvider;
        private readonly IdentityManager _identityManager;
        private readonly ParserService _parserService;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private IDisposable _backup;
        private StreamWriter _sw;

        public FileWriter(IDatasourceProvider datasourceProvider, IdentityManager identityManager, ParserService parserService)
        {
            _datasourceProvider = datasourceProvider;
            _identityManager = identityManager;
            _parserService = parserService;
            RegisterBackupObservable();
        }

        public async Task WriteAsync(StreamWriter sw)
        {
            _sw = sw;
            await foreach (var line in _datasourceProvider.ReadAsync())
            {
                await DoWriteAsync(line);
            }
        }

        private async Task DoWriteAsync(Field line)
        {
            // Write to file
            if (_parserService.TryParse(line, out var data))
            {
                // Generate Id base on fieldType
                var id = _identityManager.GetNext(line.FieldName);
                var row = $"{id},{line.FieldName},{data}";
                await _semaphoreSlim.WaitAsync();
                await _sw.WriteLineAsync(row);
                _semaphoreSlim.Release();
            }
        }

        private void RegisterBackupObservable()
        {
            _backup = Observable.Interval(TimeSpan.FromMinutes(5))
                .SelectMany(_ => Observable.FromAsync(async () =>
                {
                    await SaveCurrentFile();
                }))
                .Subscribe();
        }

        private async Task SaveCurrentFile()
        {
            await _semaphoreSlim.WaitAsync();
            await _sw.FlushAsync();
            _semaphoreSlim.Release();
        }

        private void CleanUp()
        {
            _semaphoreSlim?.Dispose();
            _backup.Dispose();
        }

        public void Dispose()
        {
            CleanUp();
        }
    }
}
