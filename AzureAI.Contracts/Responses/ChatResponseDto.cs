using AzureAI.Contracts.Dtos;

namespace AzureAI.Contracts.Responses
{
    public class ChatResponseDto
    {
        public string Response { get; set; } = string.Empty;
        public List<TraceStepDto> TraceSteps { get; set; } = new();
        public ModelStatisticsDto? ModelStatistics { get; set; }
    }
}
