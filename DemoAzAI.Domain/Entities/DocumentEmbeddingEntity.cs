using DemoAzAI.Domain.Models;

namespace DemoAzAI.Domain.Entities
{
    public class DocumentEmbeddingEntity : DocumentEntity
    {
        public List<EmbeddedChunk> EmbeddedChunks { get; set; } = new();

    }
}
 