using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatQuestionResponseDto> GetPromptResponseAsync(ChatQuestionRequestDto request, CancellationToken cancellationToken = default);
        Task<DocumentChunkingResponseDto> GetDocumentChunkingResponseAsync(DocumentChunkingRequestDto request, CancellationToken cancellationToken = default);
    }
}
