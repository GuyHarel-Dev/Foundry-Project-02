using DemoAzAI.Domain;
using System.Collections.Concurrent;

namespace DemoAzAI.Infrastructure.Stores
{
    public class EmbeddingStoreService : IEmbeddingStoreService
    {
        private static readonly ConcurrentDictionary<Guid, TextEmbedding> _store = new();

        public IReadOnlyCollection<TextEmbedding> Embeddings
            => _store.Values.ToList();

        public void AddEmbedding(TextEmbedding embedding)
        {
            _store[embedding.Guid] = embedding;
        }

        public void AddEmbeddings(IEnumerable<TextEmbedding> embeddings)
        {
            foreach (var embedding in embeddings)
            {
                _store[embedding.Guid] = embedding;
            }
        }

        public void ClearEmbeddings()
        {
            _store.Clear();
        }

        public TextEmbedding? GetEmbedding(Guid id)
        {
            _store.TryGetValue(id, out var embedding);
            return embedding;
        }
    }
}
