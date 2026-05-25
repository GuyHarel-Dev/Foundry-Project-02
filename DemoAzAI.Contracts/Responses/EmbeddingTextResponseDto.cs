namespace DemoAzAI.Contracts.Responses
{
    public class EmbeddingTextResponseDto
    {
        public string EmbeddedText { get; set; } = string.Empty;
        public Guid EmbeddedTextGuid { get; set; }
    }
}
