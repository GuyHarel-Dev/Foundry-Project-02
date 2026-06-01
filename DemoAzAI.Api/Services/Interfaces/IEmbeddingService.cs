using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using OpenAI.Embeddings;

namespace WebApi.Services.Interfaces
{
    public interface IEmbeddingService
    {
        Task<List<SemanticSearchResult>> SearchEmbeddingStoreForSimilarTextLinesAsync(string? question);
        Task<EmbeddingResponseDto> EmbeddTextLinesAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default);
    }
}
