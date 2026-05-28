using DemoAzAI.Domain;

namespace DemoAzAI.Infrastructure.Stores
{
    public interface IEmbeddingStoreService
    {
        IReadOnlyCollection<TextEmbedding> Embeddings { get; }
        void AddEmbedding(TextEmbedding storedEmbedding);
        void AddEmbeddings(IEnumerable<TextEmbedding> storedEmbedding);
        void ClearEmbeddings();
        TextEmbedding? GetEmbedding(Guid id);
    }
}
