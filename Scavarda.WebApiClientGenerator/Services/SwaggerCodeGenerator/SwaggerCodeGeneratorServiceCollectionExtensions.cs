using System;
using Scavarda.WebApiClientGenerator.Services.Abstractions.WebApiClientGenerator;
using Scavarda.WebApiClientGenerator.Services.SwaggerCodeGenerator;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerCodeGeneratorServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerCodeGeneratorService(this IServiceCollection services, Action<SwaggerCodeGeneratorServiceOptions> setupAction)
        {
            services.Configure(setupAction ?? (opts => { }));

            services.AddSingleton<IWebApiClientGeneratorService, SwaggerCodeGeneratorService>();

            return services;
        }

        public static void ConfigureSwaggerCodeGeneratorService(this IServiceCollection services, Action<SwaggerCodeGeneratorServiceOptions> setupAction)
        {
            services.Configure(setupAction ?? (opts => { }));
        }
    }
}
