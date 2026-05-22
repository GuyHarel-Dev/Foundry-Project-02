using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryEmbeddingService
    {
        Task<EmbeddingResponseDto> EmbeddTextAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default);
    }
}
