using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryEmbeddingService
    {
        Task<EmbeddingResponseDto> GetEmbeddingAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default);
        //EmbeddingResponseDto GetEmbeddingAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default);
    }
}
