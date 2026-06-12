using DemoAzAI.Infrastructure.ExternalServices.Repositories;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using WebApi.Services.Interfaces;

namespace WebApi.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddStoreServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IDocumentAnalysisRepository, SqliteDocumentAnalysisRepository>();
            services.AddSingleton<IDocumentRepository, SqliteDocumentRepository>();

            return services;
        }

        public static IServiceCollection AddFileStoreService(this IServiceCollection services, IConfiguration config)
        {

            var fileStorePath = config["DemoAzAI:LocalFileVaultPath"]
                ?? throw new InvalidOperationException("config: DemoAzAI:LocalFileVaultPath not configured");

            services.AddSingleton(sp =>
            {
                var fileStoreService = new DocumentStoreService(fileStorePath);
                return fileStoreService as IDocumentStoreService;
            });

            return services;
        }
    }
}
