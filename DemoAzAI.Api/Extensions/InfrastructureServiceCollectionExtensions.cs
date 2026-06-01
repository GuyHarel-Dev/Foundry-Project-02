using DemoAzAI.Abstractions;
using DemoAzAI.Infrastructure.ExternalServices.Stores;

namespace WebApi.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddStoreServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IDocumentAnalysisStoreService, DocumentAnalysisStoreService>();

            return services;
        }
    }
}
