using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryEmbeddingService
    {
        Task<EmbeddingTextResponseDto> EmbeddTextAsync(EmbeddingTextRequestDto request, CancellationToken cancellationToken = default);
        Task<EmbeddingTextLinesResponseDto> EmbeddTextLinesAsync(EmbeddingTextLinesRequestDto request, CancellationToken cancellationToken = default);
    }
}
