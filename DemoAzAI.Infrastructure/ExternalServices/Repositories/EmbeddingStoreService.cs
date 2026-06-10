//using DemoAzAI.Domain;
//using DemoAzAI.Abstractions;
//using System.Collections.Concurrent;

//namespace DemoAzAI.Infrastructure.ExternalServices.Stores
//{
//    public class EmbeddingStoreService : IEmbeddingStoreService
//    {
//        private static readonly ConcurrentDictionary<Guid, EmbeddedChunk> _embeddingStore = new();

//        public int Count => _embeddingStore.Count;

//        public IReadOnlyCollection<EmbeddedChunk> Embeddings
//            => _embeddingStore.Values.ToList();

//        public void AddEmbedding(EmbeddedChunk embedding)
//        {
//            _embeddingStore[embedding.Guid] = embedding;
//        }

//        public void AddEmbeddings(IEnumerable<EmbeddedChunk> embeddings)
//        {
//            foreach (var embedding in embeddings)
//            {
//                _embeddingStore[embedding.Guid] = embedding;
//            }
//        }

//        public void ClearEmbeddings()
//        {
//            _embeddingStore.Clear();
//        }

//        public EmbeddedChunk? GetEmbedding(Guid id)
//        {
//            _embeddingStore.TryGetValue(id, out var embedding);
//            return embedding;
//        }
//    }
//}
