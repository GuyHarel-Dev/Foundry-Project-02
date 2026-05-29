using DemoAzAI.Domain;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IEmbeddingStoreService
    {
        IReadOnlyCollection<EmbeddedChunk> Embeddings { get; }
        void AddEmbedding(EmbeddedChunk storedEmbedding);
        void AddEmbeddings(IEnumerable<EmbeddedChunk> storedEmbedding);
        void ClearEmbeddings();
        EmbeddedChunk? GetEmbedding(Guid id);
    }
}
