using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IEmbeddingService
    {
        Task<EmbeddingTextResponseDto> EmbedTextAsync(string text, CancellationToken cancellationToken = default);
        Task<EmbeddingTextLinesResponseDto> EmbedTextLinesAsync(EmbeddingTextLinesRequestDto request, CancellationToken cancellationToken = default);
    }
}
