namespace DemoAzAI.Domain
{
    public record EmbeddedChunk
    (
        Guid Id,
        string Text,
        float[] EmbeddingVector
    );

}
