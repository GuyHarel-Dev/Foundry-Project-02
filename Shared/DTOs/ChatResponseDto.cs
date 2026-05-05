namespace Shared.DTOs
{
    public class ChatResponseDto
    {
        public string Response { get; set; } = string.Empty;
        public List<TraceStepDto> TraceSteps { get; set; } = new();
        public AiResponseStatsDto? Stats { get; set; }
    }
}
