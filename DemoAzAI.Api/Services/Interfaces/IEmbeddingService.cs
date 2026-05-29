using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IEmbeddingService
    {
        Task SearchEmbedding(string? question);
        Task<EmbeddingResponseDto> EmbeddTextAsync(EmbeddingRequestDto request, bool storeEmbeddings = false, CancellationToken cancellationToken = default);
    }
}
