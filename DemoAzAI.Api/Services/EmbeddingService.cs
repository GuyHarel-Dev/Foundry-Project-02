using Azure.AI.Projects;
using DemoAzAI.AI;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using OpenAI.Embeddings;
using WebApi.Services.Interfaces;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class EmbeddingService(
        IConfiguration config,
        AIProjectClient projectClient,
        ILogger<EmbeddingService> logger,
        IEmbeddingStoreService embeddingStoreService,
        ISemanticSearchService semanticSearchService) : IEmbeddingService

    {

        public async Task SearchEmbedding(string? question)
        {
            // Embed the question first
            var request = new EmbeddingRequestDto
            {
                Chunks = new List<string> { question }
            };

            var response = await EmbeddTextAsync(request);

            // Retrieve the embedding vector for the question
            var questionEmbedding = response.TextEmbeddings.FirstOrDefault()?.EmbeddingVector;


            semanticSearchService.Search(embeddingStoreService, "question");


        }

        public async Task<EmbeddingResponseDto> EmbeddTextAsync(EmbeddingRequestDto request, bool storeEmbeddings = false, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Starting embedding generation for {Count} text lines.", request.Chunks.Count);

            var client = CreateEmbeddingClient();

            OpenAIEmbeddingCollection result = await client.GenerateEmbeddingsAsync(request.Chunks.ToArray(), cancellationToken: cancellationToken);

            var storedEmbeddings =  new List<EmbeddedChunk>();

            foreach (var embedding in result)
            {
                var storedEmbedding = new EmbeddedChunk(
                    Guid.NewGuid(),
                    request.Chunks[embedding.Index],
                    embedding.ToFloats().ToArray()
                );

                storedEmbeddings.Add(storedEmbedding );

            }

            if ( storeEmbeddings )
                embeddingStoreService.AddEmbeddings(storedEmbeddings);

            return new EmbeddingResponseDto
            {
                TextEmbeddings = storedEmbeddings
            };

        }

        private EmbeddingClient CreateEmbeddingClient()
        {

            var modelName = config["FoundryPrj01:Model:TextEmbedding"]
                ?? throw new InvalidOperationException("ModelName not configured");

            return projectClient
                .ProjectOpenAIClient
                .GetEmbeddingClient(modelName);
        }

    }
}

