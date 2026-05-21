using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryEmbeddingService
    {
        Task<EmbeddingResponseDto> GetEmbeddingAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default);
    }
}
