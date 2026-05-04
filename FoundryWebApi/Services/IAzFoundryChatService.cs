using SharedDTO;

namespace WebApi.Services
{
    public interface IAzFoundryChatService
    {
        Task<ChatResponse> GetResponseAsync(ChatRequest request, CancellationToken cancellationToken = default);
    }
}
