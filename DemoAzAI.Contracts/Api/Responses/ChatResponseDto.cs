using DemoAzAI.Domain.Models;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class ChatResponseDto
    {
        public string ChatResponse { get; set; } = string.Empty;
        public ModelStatistics? ModelStatistics { get; set; }
    }
}
