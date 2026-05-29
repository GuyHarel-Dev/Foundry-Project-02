using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IEmbeddingService
    {
        Task<EmbeddingTextResponseDto> EmbeddTextLinesAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default);
    }
}
