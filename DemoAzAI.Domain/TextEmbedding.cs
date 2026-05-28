namespace DemoAzAI.Domain
{
    public record TextEmbedding
    (
        Guid Guid,
        string Text,
        float[] Vector
    );
}
