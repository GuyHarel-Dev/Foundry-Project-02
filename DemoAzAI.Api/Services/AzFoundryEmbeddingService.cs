using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using DemoAzAI.Common;
using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;
using WebApi.Services.Interfaces;
using WebApi.Services.Store;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class AzFoundryEmbeddingService(
        IConfiguration _config,
        AIProjectClient _projectClient,
        ILogger<AzFoundryEmbeddingService> _logger,
        IEmbeddingStoreService _embeddingStoreService) : IAzFoundryEmbeddingService 

    {

        public async Task<EmbeddingResponseDto> EmbeddTextAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default)
        {
            var client = CreateEmbeddingClient();

            var result = await client.GenerateEmbeddingAsync(request.Text, cancellationToken: cancellationToken);

            var storedEmbedding = new StoredEmbedding(
                Guid.NewGuid(),
                request.Text,
                result.Value.ToFloats().ToArray()
            );

            _embeddingStoreService.AddEmbedding(storedEmbedding);

            return new EmbeddingResponseDto
            {
                EmbeddedText = storedEmbedding.Text,
                EmbeddedTextGuid = storedEmbedding.Id
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

