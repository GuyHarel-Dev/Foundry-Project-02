using Azure;
using Azure.AI.Projects;
using Azure.Identity;
using DemoAzAI.Api.Services;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Application.AI;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using WebApi.Services.Interfaces;

public static class BuilderServicesExtensions
{

    public static IServiceCollection AddAzureChatFoundryService(this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();

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

            OpenAI.Embeddings.EmbeddingClient  responseClient = httpClient.ProjectOpenAIClient
                .GetEmbeddingClient(modelName);

            return responseClient;
        });

        return services;

    }
    public static IServiceCollection AddDocIntelligenceService(this IServiceCollection services)
    {
        services.AddScoped<IDocumentIntelligenceService, DocumentIntelligenceService>();

        return services;
    }

    public static IServiceCollection AddEmbeddingService(this IServiceCollection services)
    {
        services.AddScoped<IEmbeddingService, EmbeddingService>();

        return services;
    }

    public static IServiceCollection AddEmbeddingStoreService(this IServiceCollection services)
    {
        services.AddScoped<IEmbeddingStoreService, EmbeddingStoreService>();

        return services;
    }

    public static IServiceCollection AddSemanticSearchService(this IServiceCollection services)
    {
        services.AddScoped<ISemanticSearchService, SemanticSearchService>();

        return services;
    }
    

}