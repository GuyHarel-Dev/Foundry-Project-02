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
                TextLines = storedEmbeddings.Select(e => e.Text).ToList(),
                Guid  = storedEmbeddings.Select(e => e.Guid).ToList(),
                FloatValuesPreview = $"[ {string.Join(", ", storedEmbeddings.FirstOrDefault()?.Vector.Take(5) ?? Array.Empty<float>())} ... ]"
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

