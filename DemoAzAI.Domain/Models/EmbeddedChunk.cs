namespace DemoAzAI.Domain.Models
{
    public record EmbeddedChunk
    (
        long ChunkIndex,
        string ChunkText,
        float[] EmbeddingVector
    );

}
