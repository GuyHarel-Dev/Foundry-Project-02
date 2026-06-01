using DemoAzAI.Abstractions;
using DemoAzAI.Domain;
using DemoAzAI.Infrastructure.Persistence;

namespace WebApi.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            services.AddScoped<
                IHashEntityStore<DocumentAnalysisResult>,
                SqliteDocumentAnalysisStore>();

            return services;
        }
    }
}
