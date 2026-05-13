using AzureAI.Contracts.Dtos;

namespace AzureAI.Contracts.Responses
{
    public class ChatResponseDto
    {
        public string Response { get; set; } = string.Empty;
        public ModelStatisticsDto? ModelStatistics { get; set; }
    }
}
