namespace DemoAzAI.Contracts.Responses
{
    public class EmbeddingResponseDto
    {
        public string EmbeddedText { get; set; } = string.Empty;
        public Guid EmbeddedTextGuid { get; set; }
    }
}
