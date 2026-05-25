namespace WebApi.Services.Store
{
    public record StoredEmbedding(
        Guid Guid,
        string Text,
        float[] Vector
    );
}
