using DemoAzAI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzAI.Infrastructure.Stores
{
    public class EmbeddingStoreService : IEmbeddingStoreService
    {
        public List<TextEmbedding> Embeddings { get; set; } = new List<TextEmbedding>();
        IReadOnlyCollection<TextEmbedding> IEmbeddingStoreService.Embeddings { get => Embeddings; }

        public void AddEmbeddings(IEnumerable<TextEmbedding> storedEmbedding)
        {
            Embeddings.AddRange(storedEmbedding);
        }

        public void AddEmbedding(TextEmbedding storedEmbedding)
        {
            Embeddings.Add(storedEmbedding);
        }

        public void ClearEmbeddings()
        {
            Embeddings.Clear();
        }

        TextEmbedding? IEmbeddingStoreService.GetEmbedding(Guid id)
        {
            return Embeddings.FirstOrDefault(e => e.Guid == id);
        }
    }
}
