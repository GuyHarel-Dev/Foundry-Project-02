using Shared.DTOs;

namespace Contracts.Requests
{
    public class ChatRequestDto
    {
        public string Prompt { get; set; } = string.Empty;
        public List<TraceStepDto> TraceSteps { get; set; } = new ();
    }
}
