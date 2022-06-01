using System.Collections.Generic;
using UserCreator.Core.Contracts;

namespace UserCreator.Core.Providers
{
    /// <summary>
    /// Provide input data which will be stored in CSV files
    /// </summary>
    public interface IDatasourceProvider
    {
        IAsyncEnumerable<Field> ReadAsync();
    }
}