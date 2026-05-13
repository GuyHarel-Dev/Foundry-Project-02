using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;

namespace AzureAI.Api.Services.Interfaces
{
    public interface IAzFoundryChatService
    {
        Task<ChatResponseDto> GetResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
    }
}
