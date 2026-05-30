namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddedChunkDto
    {
        public Guid Guid { get; set; }
        public string? Text { get; set; }
        public string? EmbeddingVectorView { get; set; }
    }
}
