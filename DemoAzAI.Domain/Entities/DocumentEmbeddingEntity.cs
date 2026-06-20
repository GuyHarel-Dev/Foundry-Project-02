namespace DemoAzAI.Domain.Entities
{
    public class DocumentEmbeddingEntity
    {
        public string? ChunkText { get; set; }
        public string? EmbeddingVectorJson { get; set; }
        public float[]? EmbeddingVector { get; set; }

        public float CosineSimilarity { get; set; }

    }
}
