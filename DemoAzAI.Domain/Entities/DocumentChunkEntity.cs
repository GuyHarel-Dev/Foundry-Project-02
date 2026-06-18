using DemoAzAI.Domain.Models;

namespace DemoAzAI.Domain.Entities
{
    public class DocumentChunkEntity : DocumentEntity
    {
        public List<EmbeddedChunk> EmbeddedChunks { get; set; } = new();

    }
}
 