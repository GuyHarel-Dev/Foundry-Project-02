using Shared.DTOs;

namespace WebApi.Services.Interfaces
{
    public interface IAzFoundryChatService
    {
        Task<ChatResponseDto> GetResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
    }
}
