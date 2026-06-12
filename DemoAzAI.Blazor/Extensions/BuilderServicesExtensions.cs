using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;

namespace UI.Extensions
{
    static public class BuilderServicesExtensions
    {
        public static IServiceCollection AddFoundryHttpClient(this IServiceCollection services, IConfiguration config)
        {
            // Extract the base URL from configuration or use a default value
            var baseUrl = config["FoundryWebApi:BaseUrl"] ?? "https://localhost:5001"; // Default URL if not specified in configuration

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            services.AddScoped(sp =>
              new ApiClient(baseUrl, httpClient));

            return services;
        }

        public static IServiceCollection AddApplicationStepTracer(this IServiceCollection services)
        {
            services.AddScoped<IExecutionStepTracer, ExecutionStepTracer>();

            return services;
        }

    }
}
