using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.AI.Projects;
using Azure.Identity;
using DemoAzAI.AI;
using DemoAzAI.Api.Services;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Application.AI;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using WebApi.Services.Interfaces;

public static class BuilderServicesExtensions
{

    public static IServiceCollection AddAzFoundryService(this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();

        return services;
    }

    public static IServiceCollection AddAzFoundryHttpClient(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();

            var endpoint = config["FoundryPrj01:Endpoint"]
                ?? throw new InvalidOperationException("FoundryPrj01 Endpoint not configured");

            return new AIProjectClient(
                endpoint: new Uri(endpoint),
                tokenProvider: new DefaultAzureCredential());
        });


        return services;
    }

    public static IServiceCollection AddAzDocIntelligenceClient(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();

            var endpoint = config["AzAiDocIntelligenceService:Endpoint"]
                ?? throw new InvalidOperationException("AzureDocIntelligence Endpoint not configured");

            var key = config["AzAiDocIntelligenceService:Key1"]
                ?? throw new InvalidOperationException("AzureDocIntelligence Key1 not configured");

            AzureKeyCredential credential = new AzureKeyCredential(key);

            return new DocumentIntelligenceClient(
                endpoint: new Uri(endpoint),
                credential: credential);
        });


        return services;
    }

    public static IServiceCollection AddAzAiDocIntelligenceService(this IServiceCollection services)
    {
        services.AddScoped<IDocumentIntelligenceService, DocumentIntelligenceService>();

        return services;
    }

    public static IServiceCollection AddAzFoundryEmbeddingService(this IServiceCollection services)
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