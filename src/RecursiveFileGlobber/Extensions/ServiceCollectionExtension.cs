using Microsoft.Extensions.DependencyInjection;

namespace RecursiveFileGlobber.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection SetupFileGlobberServices(this IServiceCollection services)
        {
            return services;
        }
    }
}