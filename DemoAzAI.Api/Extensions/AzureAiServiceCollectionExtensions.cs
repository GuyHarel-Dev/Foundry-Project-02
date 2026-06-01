using Azure;
using Azure.AI.Projects;
using Azure.Identity;

namespace WebApi.Extensions
{
    public static class AzureAiServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureAiServices(this IServiceCollection services, IConfiguration config)
        {
            AddAzureOpenAiProjectResponsesClient(services, config);
            AddAzureDocIntelligenceClient(services);
            AddAzureOpenAiEmbeddingClient(services, config);

            return services;
        }

        public static IServiceCollection AddAzureOpenAiProjectResponsesClient(this IServiceCollection services, IConfiguration config)
        {

            var endpoint = config["FoundryPrj01:Endpoint"]
                ?? throw new InvalidOperationException("config: FoundryPrj01:Endpoint not configured");

            var modelName = config["FoundryPrj01:Model:OpenAiChat"]
                ?? throw new InvalidOperationException("config: FoundryPrj01:Model:OpenAiChat");

            services.AddSingleton(sp =>
            {
                var httpClient = new AIProjectClient(
                endpoint: new Uri(endpoint),
                tokenProvider: new DefaultAzureCredential());

                Azure.AI.Extensions.OpenAI.
                    ProjectResponsesClient responseClient = httpClient.ProjectOpenAIClient
                    .GetProjectResponsesClientForModel(modelName);

                return responseClient;
            });

            return services;
        }

        public static IServiceCollection AddAzureDocIntelligenceClient(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();

                var endpoint = config["AzAiDocIntelligenceService:Endpoint"]
                    ?? throw new InvalidOperationException("config: AzAiDocIntelligenceService:Endpoint");

                var key = config["AzAiDocIntelligenceService:Key1"]
                    ?? throw new InvalidOperationException("config: AzAiDocIntelligenceService:Key1");

                AzureKeyCredential credential = new AzureKeyCredential(key);

                return new Azure.AI.DocumentIntelligence
                    .DocumentIntelligenceClient(
                        endpoint: new Uri(endpoint),
                        credential: credential);
            });


            return services;
        }

        public static IServiceCollection AddAzureOpenAiEmbeddingClient(this IServiceCollection services, IConfiguration config)
        {

            var endpoint = config["FoundryPrj01:Endpoint"]
                ?? throw new InvalidOperationException("config: FoundryPrj01:Endpoint not configured");

            var modelName = config["FoundryPrj01:Model:TextEmbedding"]
                ?? throw new InvalidOperationException("config: FoundryPrj01:Model:TextEmbedding\" not configured");

            services.AddSingleton(sp =>
            {
                var httpClient = new AIProjectClient(
                endpoint: new Uri(endpoint),
                tokenProvider: new DefaultAzureCredential());

                OpenAI.Embeddings.EmbeddingClient responseClient = httpClient.ProjectOpenAIClient
                    .GetEmbeddingClient(modelName);

                return responseClient;
            });

            return services;

        }
    }
}
