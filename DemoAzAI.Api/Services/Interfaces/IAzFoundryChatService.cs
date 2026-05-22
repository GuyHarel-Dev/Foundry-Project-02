using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IAzFoundryChatService
    {
        Task<ChatResponseDto> GetPromptResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
    }
}
