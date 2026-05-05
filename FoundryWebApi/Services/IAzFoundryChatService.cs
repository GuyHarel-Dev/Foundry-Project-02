using Shared.DTOs;

namespace WebApi.Services
{
    public interface IAzFoundryChatService
    {
        Task<ChatResponseDto> GetResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
    }
}
