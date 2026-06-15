using DemoAzAI.Infrastructure.ExternalServices.Repositories;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using DemoAzAI.Infrastructure.ExternalServices.Stores;

namespace WebApi.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration config)
        {
            var repositoryPath = config["DemoAzAI:LocalRepositoryPath"]
                ?? throw new InvalidOperationException("config: DemoAzAI:LocalRepositoryPath not configured");

            services.AddSingleton(sp =>
            {
                var repositoryService = new SqliteDocumentAnalysisRepository(repositoryPath);
                return repositoryService as IDocumentAnalysisRepository;

            });

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
