using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using OpenAI.Embeddings;

namespace WebApi.Services.Interfaces
{
    public interface IEmbeddingService
    {
        int EmbeddingStoreCount { get; }

        Task<List<SemanticSearchResult>?> SearchEmbedding(string? question);
        Task<EmbeddingResponseDto> EmbeddTextAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default);
    }
}
