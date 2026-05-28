using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryEmbeddingService
    {
        Task<EmbeddingTextResponseDto> EmbeddTextLinesAsync(EmbeddingTextRequestDto request, CancellationToken cancellationToken = default);
    }
}
