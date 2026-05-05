namespace Shared.DTOs
{
    public class ChatRequestDto
    {
        public string Prompt { get; set; } = string.Empty;
        public List<TraceStepDto> TraceSteps { get; set; } = new ();
    }
}
