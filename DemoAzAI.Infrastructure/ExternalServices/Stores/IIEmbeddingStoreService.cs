using DemoAzAI.Domain;

namespace DemoAzAI.Infrastructure.ExternalServices.Stores
{
    public interface IEmbeddingStoreService
    {
        int Count { get; }
        IReadOnlyCollection<EmbeddedChunk> Embeddings { get; }
        void AddEmbedding(EmbeddedChunk storedEmbedding);
        void AddEmbeddings(IEnumerable<EmbeddedChunk> storedEmbedding);
        void ClearEmbeddings();
        EmbeddedChunk? GetEmbedding(Guid id);
    }
}
