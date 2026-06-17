using DemoAzAI.Api.Services;
using DemoAzAI.Api.Services.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Extensions
{
    public static class ApiServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();

            services.AddScoped<IDocumentService,
                DocumentService>();

            services.AddScoped<IEmbeddingService,
                EmbeddingService>();

            //services.AddScoped<ISemanticSearchService,
            //    SemanticSearchService>();

            return services;
        }
    }
}
