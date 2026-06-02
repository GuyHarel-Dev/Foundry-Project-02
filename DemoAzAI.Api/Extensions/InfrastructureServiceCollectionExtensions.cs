using DemoAzAI.Abstractions;
using DemoAzAI.Infrastructure.Persistence;

namespace WebApi.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddStoreServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IDocumentAnalysisStoreService, SqliteDocumentAnalysisStore>();

            return services;
        }
    }
}
