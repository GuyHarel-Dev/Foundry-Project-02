using DemoAzAI.Domain;

namespace DemoAzAI.Api.Contracts.Responses
{
    public class ChatResponseDto
    {
        public string ChatResponse { get; set; } = string.Empty;
        public ModelStatistics? ModelStatistics { get; set; }
    }
}
