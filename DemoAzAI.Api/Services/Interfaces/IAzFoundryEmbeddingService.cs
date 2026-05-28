using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryEmbeddingService
    {
        Task<EmbeddingTextResponseDto> EmbeddTextLinesAsync(EmbeddingTextRequestDto request, CancellationToken cancellationToken = default);
    }
}
