using DemoAzAI.Abstractions;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddStoreServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IDocumentAnalysisStoreService, DocumentAnalysisStoreService>();
            services.AddSingleton<IStoreService, StoreService>();

            return services;
        }
    }
}
