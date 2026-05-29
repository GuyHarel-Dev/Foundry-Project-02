namespace DemoAzAI.Domain
{
    public record EmbeddedChunk
    (
        Guid Guid,
        string Text,
        float[] EmbeddingVector
    );
}
