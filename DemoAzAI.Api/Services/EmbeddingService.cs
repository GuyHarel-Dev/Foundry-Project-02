using Azure.AI.Projects;
using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;
using DemoAzAI.Domain;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using OpenAI.Embeddings;
using WebApi.Services.Interfaces;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class EmbeddingService(
        IConfiguration _config,
        AIProjectClient _projectClient,
        ILogger<EmbeddingService> _logger,
        IEmbeddingStoreService _embeddingStoreService) : IEmbeddingService

    {

        public async Task<EmbeddingTextResponseDto> EmbeddTextLinesAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting embedding generation for {Count} text lines.", request.Chunks.Count);

            var client = CreateEmbeddingClient();
            _embeddingStoreService.ClearEmbeddings();

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

            _embeddingStoreService.AddEmbeddings(storedEmbeddings);

            return new EmbeddingTextResponseDto
            {
                TextEmbeddings = storedEmbeddings
            };

        }

        private EmbeddingClient CreateEmbeddingClient()
        {

            var modelName = _config["FoundryPrj01:Model:TextEmbedding"]
                ?? throw new InvalidOperationException("ModelName not configured");

            return _projectClient
                .ProjectOpenAIClient
                .GetEmbeddingClient(modelName);
        }

    }
}

