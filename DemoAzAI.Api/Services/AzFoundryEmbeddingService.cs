using Azure.AI.Projects;
using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;
using DemoAzAI.Domain;
using DemoAzAI.Infrastructure.Stores;
using OpenAI.Embeddings;
using WebApi.Services.Interfaces;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class AzFoundryEmbeddingService(
        IConfiguration _config,
        AIProjectClient _projectClient,
        ILogger<AzFoundryEmbeddingService> _logger,
        IEmbeddingStoreService _embeddingStoreService) : IAzFoundryEmbeddingService

    {

        public async Task<EmbeddingTextResponseDto> EmbeddTextLinesAsync(EmbeddingTextRequestDto request, CancellationToken cancellationToken = default)
        {
            var client = CreateEmbeddingClient();
            _embeddingStoreService.ClearEmbeddings();

            OpenAIEmbeddingCollection result = await client.GenerateEmbeddingsAsync(request.TextLines.ToArray(), cancellationToken: cancellationToken);

            var storedEmbeddings =  new List<TextEmbedding>();

            foreach (var embedding in result)
            {
                var storedEmbedding = new TextEmbedding(
                    Guid.NewGuid(),
                    request.TextLines[embedding.Index],
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

