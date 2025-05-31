using Microsoft.Extensions.DependencyInjection;

namespace FileGlobber.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection SetupFileGlobberServices(this IServiceCollection services)
        {
            return services;
        }
    }
}