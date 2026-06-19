namespace DemoAzAI.Domain.Entities
{
    public class DocumentEmbeddingEntity
    {
        public string? Id { get; set; }
        public string? DocumentId { get; set; }
        public long? ChunkIndex { get; set; }
        public string? ChunkText { get; set; }
        public string? EmbeddingVectorJson { get; set; }

        public float[]? EmbeddingVector { get; set; }
    }
}
 