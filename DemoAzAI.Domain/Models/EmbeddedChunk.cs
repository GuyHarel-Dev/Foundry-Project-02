namespace DemoAzAI.Domain.Models
{
    public record EmbeddedChunk
    (
        Guid Id,
        string Text,
        float[] EmbeddingVector
    );

}
