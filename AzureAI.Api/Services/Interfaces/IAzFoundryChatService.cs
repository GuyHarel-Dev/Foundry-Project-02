using Contracts.Requests;
using Contracts.Responses;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryChatService
    {
        Task<ChatResponseDto> GetResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
    }
}
