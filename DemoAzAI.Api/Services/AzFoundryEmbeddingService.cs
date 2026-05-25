using Azure.AI.Projects;
using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;
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

        public async Task<EmbeddingTextResponseDto> EmbeddTextAsync(EmbeddingTextRequestDto request, CancellationToken cancellationToken = default)
        {
            var client = CreateEmbeddingClient();

            OpenAIEmbedding result = await client.GenerateEmbeddingAsync(request.Text, cancellationToken: cancellationToken);

            var storedEmbedding = new StoredEmbedding(
                Guid.NewGuid(),
                request.Text,
                result.ToFloats().ToArray()
            );

            _embeddingStoreService.AddEmbedding(storedEmbedding);

            return new EmbeddingTextResponseDto
            {
                EmbeddedText = storedEmbedding.Text,
                EmbeddedTextGuid = storedEmbedding.Guid
            };

        }

        public async Task<EmbeddingTextLinesResponseDto> EmbeddTextLinesAsync(EmbeddingTextLinesRequestDto request, CancellationToken cancellationToken = default)
        {
            var client = CreateEmbeddingClient();
            _embeddingStoreService.ClearEmbeddings();

            OpenAIEmbeddingCollection result = await client.GenerateEmbeddingsAsync(request.TextLines.ToArray(), cancellationToken: cancellationToken);

            var storedEmbeddings =  new List<StoredEmbedding>();

            foreach (var embedding in result)
            {
                var storedEmbedding = new StoredEmbedding(
                    Guid.NewGuid(),
                    request.TextLines[embedding.Index],
                    embedding.ToFloats().ToArray()
                );

                storedEmbeddings.Add(storedEmbedding );

            }

            _embeddingStoreService.AddEmbeddings(storedEmbeddings);

            return new EmbeddingTextLinesResponseDto
            {
                EmbeddedTextLines = storedEmbeddings.Select(e => e.Text).ToList(),
                EmbeddedTextLinesGuids  = storedEmbeddings.Select(e => e.Guid).ToList()
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

