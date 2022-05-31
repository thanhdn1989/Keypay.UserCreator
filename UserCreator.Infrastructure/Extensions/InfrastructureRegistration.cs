using Microsoft.Extensions.DependencyInjection;
using UserCreator.Core.Providers;
using UserCreator.Infrastructure.Providers;

namespace UserCreator.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static void RegisterInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<IDatasourceProvider, ConsoleDatasourceProvider>();
        }
    }
}