using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryEmbeddingService
    {
        Task<EmbeddingTextResponseDto> EmbeddTextLinesAsync(EmbeddingTextLRequestDto request, CancellationToken cancellationToken = default);
    }
}
