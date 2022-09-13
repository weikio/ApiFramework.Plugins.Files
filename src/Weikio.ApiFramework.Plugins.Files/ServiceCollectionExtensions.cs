using Microsoft.Extensions.DependencyInjection;
using Weikio.ApiFramework.Abstractions.DependencyInjection;
using Weikio.ApiFramework.SDK;

namespace Weikio.ApiFramework.Plugins.Files
{
    public static class ServiceExtensions
    {
        public static IApiFrameworkBuilder AddFilesApi(this IApiFrameworkBuilder builder, string endpoint = null, FileApiConfiguration configuration = null)
        {
            builder.Services.AddFilesApi(endpoint, configuration);

            return builder;
        }

        public static IServiceCollection AddFilesApi(this IServiceCollection services, string endpoint = null, FileApiConfiguration configuration = null)
        {
            services.RegisterPlugin(endpoint, configuration);

            return services;
        }
    }
}
