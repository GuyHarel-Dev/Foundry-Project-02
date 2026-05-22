namespace DemoAzAI.Contracts.Responses
{
    public class EmbeddingResponseDto
    {
        public string RequestId { get; set; } = string.Empty;
        public string ResponseId { get; set; } = string.Empty;
        //public List<float> EmbeddingVector { get; set; } = new();

    }
}
