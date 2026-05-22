using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;
using WebApi.Services.Interfaces;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class AzFoundryEmbeddingService : IAzFoundryEmbeddingService

    {
        private readonly IConfiguration _config;
        private readonly AIProjectClient _projectClient;
        private readonly ILogger<AzFoundryEmbeddingService> _logger;

        public AzFoundryEmbeddingService(
            IConfiguration config,
            AIProjectClient projectClient,
            ILogger<AzFoundryEmbeddingService> logger)
        {
            _config = config;
            _projectClient = projectClient;
            _logger = logger;

        }


        public async Task<EmbeddingResponseDto> GetEmbeddingAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default)
        {
            var client = CreateEmbeddingClient();

            var result = await client.GenerateEmbeddingAsync(request.Text, cancellationToken: cancellationToken);
            return new EmbeddingResponseDto
            {
                EmbeddingVector = result.Value.ToFloats()
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

