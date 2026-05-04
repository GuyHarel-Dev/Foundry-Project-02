namespace SharedDTO
{
    public class ChatResponse
    {
        public string Response { get; set; } = string.Empty;
        public List<ExecutionTraceStepDto> Trace { get; set; } = new();
    }
}
