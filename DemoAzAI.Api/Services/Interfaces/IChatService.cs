using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;

namespace DemoAzAI.Api.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponseDto> GetPromptResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
    }
}
