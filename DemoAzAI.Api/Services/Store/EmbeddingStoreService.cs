using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Services.Store
{
    public record StoredEmbedding(
        Guid Id,
        string Text,
        float[] Vector
    );

    public class EmbeddingStoreService : IEmbeddingStoreService
    {
        public List<StoredEmbedding> Embeddings { get; set; } = new List<StoredEmbedding>();

        public void AddEmbedding(StoredEmbedding storedEmbedding)
        {
            Embeddings.Add(storedEmbedding);
        }

        public void ClearEmbeddings()
        {
            Embeddings.Clear();
        }

        StoredEmbedding? IEmbeddingStoreService.GetEmbedding(Guid id)
        {
            return Embeddings.FirstOrDefault(e => e.Id == id);
        }
    }
}
