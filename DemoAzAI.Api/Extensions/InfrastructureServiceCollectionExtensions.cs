using DemoAzAI.Abstractions.Repositories;
using DemoAzAI.Infrastructure.Documents;
using DemoAzAI.Infrastructure.ExternalServices.Stores;

namespace WebApi.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddStoreServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IDocumentAnalysisRepository, SqliteDocumentAnalysisRepository>();
            services.AddSingleton<IDocumentRepository, DocumentRepository>();

            return services;
        }
    }
}
