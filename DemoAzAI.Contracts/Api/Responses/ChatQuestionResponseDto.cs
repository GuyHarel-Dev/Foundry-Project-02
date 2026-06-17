using DemoAzAI.Domain.Models;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class ChatQuestionResponseDto
    {
        public string ChatResponse { get; set; } = string.Empty;
        public ModelStatistics? ModelStatistics { get; set; }
    }

}
