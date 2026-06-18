using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;

namespace WebApi.Services.Interfaces
{
    public interface IEmbeddingService
    {
        Task<EmbeddingResponseDto> EmbedTextAsync(string text, CancellationToken cancellationToken = default);
        Task<EmbeddingResponseDto> EmbedTextLinesAsync(EmbeddingTextLinesRequestDto request, CancellationToken cancellationToken = default);
    }
}
