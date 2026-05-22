namespace DemoAzAI.Contracts.Responses
{
    public class EmbeddingResponseDto
    {
        //public EmbeddingResponseDto(ReadOnlyMemory<float> floats)
        //{
        //    EmbeddingVector = [.. floats.Span];
        //}

        public ReadOnlyMemory<float> EmbeddingVector { get; init; }
    }
}
